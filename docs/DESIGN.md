# HeartSlots — дизайн-документ

Мод для tModLoader. Версия игры пользователя: **Terraria 1.4.4.9**, **tModLoader ветка `TML_2026_08`**.

> Документ отражает **фактическое** состояние API, проверенное компилятором и рефлексией по
> `tModLoader.dll`, а не только замысел. Раздел 12 — что реально собралось.

---

## 1. Суть

Четыре крафтуемых расходуемых предмета. Каждый при **первом** использовании навсегда открывает
персонажу **+1 слот аксессуаров**, после чего исчезает.

Ключевое правило (как ванильное Демоническое сердце): предмет одноразовый по эффекту.
Скрафтить второй такой же можно, но использовать его бессмысленно — слот уже открыт,
предмет **не потратится**.

Четыре слота **независимы**: любой можно открыть первым, порядок не важен.

---

## 2. Проверенное окружение

| Параметр | Значение |
|---|---|
| Папка игры | `<Steam>\steamapps\common\tModLoader` |
| Версия игры | 1.4.4.9 (версия сборки `tModLoader.dll`) |
| Ветка tML | `TML_2026_08` |
| Цель сборки | `net8.0`, `LangVersion` 12.0 |
| .NET рантайм | 8.0.31 установлен |
| Файл целей MSBuild | `tMLMod.targets` в корне папки игры |

Проект собирается через `dotnet build` из папки `HeartSlots`.

**Важно:** tModLoader берёт внутреннее имя мода из **имени папки проекта**. Поэтому папка
называется `HeartSlots` — от этого зависят ключи локализации (`Mods.HeartSlots.*`) и пути к
текстурам (`HeartSlots/Assets/Items/*`).

---

## 3. Предметы

| # | Класс | Русское имя | Английское имя | Редкость |
|---|---|---|---|---|
| 1 | `CrystalHeart` | Кристальное Сердце | Crystal Heart | Blue (1) |
| 2 | `OminousTalisman` | Зловещий Талисман | Ominous Talisman | Green (2) |
| 3 | `MechanicalExpander` | Механический Расширитель | Mechanical Expander | Pink (5) |
| 4 | `CelestialSynthesizer` | Небесный Синтезатор | Celestial Synthesizer | Purple (11) |

Все четыре: `consumable = true`, `maxStack = 1`, `useStyle = ItemUseStyleID.EatFood`,
`UseSound = SoundID.Item29`, `ResearchUnlockCount = 1`.

### 3.1 Рецепты

Все рецепты — **на алтаре** (`TileID.DemonAltar`, он же Багряный алтарь: tile ID 26 у обоих).

**1. Кристальное Сердце**
- 10 × Кристалл жизни (`ItemID.LifeCrystal`)
- 10 × Алмаз (`ItemID.Diamond`)
- 10 × Сапфир (`ItemID.Sapphire`)
- 10 × Аметист (`ItemID.Amethyst`)

**2. Зловещий Талисман**
- 10 × группа «Злая чешуя»: Чешуйка тени `ItemID.ShadowScale` / Образец ткани `ItemID.TissueSample`
- 10 × группа «Злой слиток»: Демонит `ItemID.DemoniteBar` / Кримтанит `ItemID.CrimtaneBar`
- 50 × Кость (`ItemID.Bone`)
- 1 × группа «Наковальня»: Железная `ItemID.IronAnvil` / Свинцовая `ItemID.LeadAnvil`

**3. Механический Расширитель**
- 15 × Слиток хлорофита (`ItemID.ChlorophyteBar`)
- 15 × Святой слиток (`ItemID.HallowedBar`)
- 5 × Душа света (`ItemID.SoulofLight`)
- 5 × Душа ночи (`ItemID.SoulofNight`)
- 1 × группа «Механический призыватель»: `ItemID.MechanicalEye` / `ItemID.MechanicalWorm` / `ItemID.MechanicalSkull`

**4. Небесный Синтезатор**
- 20 × Слиток люминита (`ItemID.LunarBar` — именно так называется слиток люминита)
- 10 × Осколок солнца (`ItemID.FragmentSolar`)
- 10 × Осколок вихря (`ItemID.FragmentVortex`)
- 10 × Осколок туманности (`ItemID.FragmentNebula`)
- 10 × Осколок пыли (`ItemID.FragmentStardust`)

### 3.2 Группы рецептов

Четыре группы регистрируются модом через `ModSystem.AddRecipeGroups()` и
`RecipeGroup.RegisterGroup(string, RecipeGroup)`:

- `HeartSlots:EvilScale`
- `HeartSlots:EvilBar`
- `HeartSlots:Anvil`
- `HeartSlots:MechSummon`

Тип `RecipeGroup` лежит в пространстве имён **`Terraria`**, не `Terraria.ModLoader`.

---

## 4. Поведение

### 4.1 Состояние

`Common/HeartSlotsPlayer.cs` — `ModPlayer` с четырьмя булевыми полями
(`crystalHeart`, `ominousTalisman`, `mechanicalExpander`, `celestialSynthesizer`),
сериализуемыми в один битмаск `unlocks`.

- `SaveData` / `LoadData` — сохраняется в файл персонажа.
- `CopyClientState` — копирование между клонами игрока.
- `SendClientChanges` + `SyncPlayer(int, int, bool)` + `Mod.HandlePacket` — синхронизация
  в мультиплеере (тип сообщения `HeartSlotsMessageType.SyncUnlocks`).

### 4.2 Слоты

`Content/Slots/` — четыре наследника `ModAccessorySlot`:
`CrystalSlot`, `OminousSlot`, `MechanicalSlot`, `CelestialSlot`.

- `IsEnabled()` возвращает соответствующий флаг.
- `DrawFunctionalSlot => true`, `DrawVanitySlot => false`, `DrawDyeSlot => false` —
  только рабочая ячейка. По умолчанию слот рисует ещё ячейку красоты и ячейку красителя,
  то есть 4 слота превратились бы в 12 ячеек и колонка уехала бы за экран.
- `CanAcceptItem` не переопределяется — любой аксессуар подходит.
- `IsVisibleWhenNotEnabled()` по умолчанию `!IsEmpty`, поэтому закрытый слот не рисуется.

### 4.3 Использование предмета

- `CanUseItem` возвращает `false`, если слот уже открыт. Это гарантирует, что предмет
  **не будет потрачен** (проверка происходит до начала использования).
- `UseItem` ставит флаг, пишет сообщение в чат (только локальному игроку) и возвращает `true`.

### 4.4 Подсказка в тултипе

`ModifyTooltips` добавляет строку «Навсегда открывает +1 слот» или «Этот слот уже открыт».

---

## 5. Ресурсы

Спрайты нарезаны из исходного листа 1254×1254 (настоящая альфа, сетка 2×2). Сам лист и
скрипты нарезки в репозиторий не входят — в нём только готовые ассеты.

Финальный формат: **48×48**, PNG, альфа, LANCZOS (NEAREST на этом исходнике даёт шум).
Раскладка: `Assets/Items/<Class>.png`, путь задаётся явно через `Texture`.

Иконка мода: `icon.png` в корне мода, **80×80** (жёсткий лимит tModLoader), обрезана по
содержимому и вписана в квадрат с полями.

---

## 6. Локализация

`Localization/en-US.hjson` и `Localization/ru-RU.hjson`.
Ключи: `Mods.HeartSlots.Items.<Class>.DisplayName` / `.Tooltip`,
плюс `Mods.HeartSlots.Misc.*` для сообщений и строк тултипа.

---

## 7. Совместимость с Recipe Browser

Отдельной работы не требуется: рецепты регистрируются штатно через `Recipe.Create(...)`,
и Recipe Browser читает `Main.recipe` вместе с ванильными группами рецептов.

---

## 8. Файловая структура

```
.
├── README.md
├── LICENSE
├── release/HeartSlots.tmod         # готовый мод, кладётся в папку Mods
├── docs/
│   ├── DESIGN.md                   # этот документ
│   └── img/*.png                   # картинки для README
└── HeartSlots/                     # ← сам мод, имя папки = внутреннее имя мода
    ├── build.txt
    ├── HeartSlots.csproj
    ├── icon.png                    # иконка мода, 80×80
    ├── HeartSlots.cs               # Mod + HandlePacket
    ├── Common/
    │   ├── HeartSlotsMessageType.cs
    │   └── HeartSlotsPlayer.cs
    ├── Content/
    │   ├── HeartSlotsRecipes.cs    # группы рецептов + рецепты
    │   ├── Items/
    │   │   ├── SlotUnlockItem.cs   # общая логика
    │   │   └── {CrystalHeart,OminousTalisman,MechanicalExpander,CelestialSynthesizer}.cs
    │   └── Slots/
    │       ├── HeartSlot.cs        # общая база
    │       └── {CrystalSlot,OminousSlot,MechanicalSlot,CelestialSlot}.cs
    ├── Assets/Items/*.png
    └── Localization/{en-US,ru-RU}.hjson
```

---

## 9. Известные риски

1. **Алтари нельзя переносить** — за крафтом нужно идти в Искажение/Багрянку. Принято осознанно.
2. **Колонка слотов может уехать за экран** при высоком UI-масштабе (tModLoader issue #1911).
   Митигация: одна ячейка на слот вместо трёх.
3. **Скрытие ячеек красоты и красителя** — надо проверить в игре, что вёрстка слота
   не поехала (осталось ли место под невидимые ячейки).

---

## 10. Вне рамок (сознательно не делаем)

- Конфиг-файлы и переключатели в UI.
- Свои текстуры фона слотов.
- Дроп предметов с боссов / альтернативные способы получения.

---

## 11. Критерии приёмки

- [x] `dotnet build` проходит без ошибок и предупреждений, на выходе есть `.tmod`.
- [ ] Все четыре предмета появляются в игре с русскими именами и описаниями.
- [ ] Каждый предмет открывает ровно один слот.
- [ ] Повторное использование того же предмета слот не добавляет и предмет не тратит.
- [ ] Открытые слоты сохраняются после выхода и перезахода в мир.
- [ ] Рецепты видны и работают на алтаре, включая варианты «или».

---

## 12. Фактическое состояние сборки

Собрано 02.11.2026, `dotnet build`: **0 ошибок, 0 предупреждений**.
Артефакт: `HeartSlots.tmod`, 50 294 байта (с иконкой), в папке модов игры.

Метаданные: автор — `Arima`, отображаемое имя — `HeartSlots`, версия 1.0.
Статичный тултип предметов содержит только «вкусовой» текст, а функциональная строка
(«Навсегда открывает +1 слот» / «Этот слот уже открыт») добавляется динамически —
так описание не дублируется.

Расхождения между замыслом и реальным API, пойманные компилятором:

| Замысел | Реальность |
|---|---|
| `ItemUseStyleID.Grab` | Такого значения нет. Доступны: `None`, `Swing`, `EatFood`, `Thrust`, `HoldUp`, `Shoot`, `DrinkLong`, `DrinkOld`, `GolfPlay`, `DrinkLiquid`, `HiddenAnimation`, `MowTheLawn`, `Guitar`, `Rapier`, `RaiseLamp`. Использован `EatFood`. |
| `Item.useSound` | Поле называется `UseSound` и имеет тип `SoundStyle?`. |
| `Item.sellPrice(...)` как метод экземпляра | Метод **статический**, вызывается как `Terraria.Item.sellPrice(...)`. |
| `ItemID.LuminiteBar` | Слиток люминита — `ItemID.LunarBar` (3467). |
| `ModAccessorySlot.FunctionalSlot` / `.VanitySlot` | Таких членов нет. Управление ячейками — через `DrawFunctionalSlot` / `DrawVanitySlot` / `DrawDyeSlot`. |
| `Terraria.ModLoader.RecipeGroup` | Тип лежит в `Terraria.RecipeGroup`; регистрация — `RecipeGroup.RegisterGroup(string, RecipeGroup)`. |
| Имя папки проекта | Внутреннее имя мода берётся из имени папки, а не из `build.txt`. Папка обязана называться `HeartSlots`, иначе сломаются ключи локализации и пути к текстурам. |
