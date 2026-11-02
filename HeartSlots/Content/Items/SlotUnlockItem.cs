using System.Collections.Generic;
using HeartSlots.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HeartSlots.Content.Items;

/// <summary>
/// Shared behaviour for every "permanently unlock one accessory slot" consumable.
/// Derived classes only declare which flag they flip.
/// </summary>
public abstract class SlotUnlockItem : ModItem
{
	/// <summary>Ref-returning accessor so the base class can read and write the flag.</summary>
	protected abstract ref bool Unlocked(HeartSlotsPlayer modPlayer);

	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 48;
		Item.maxStack = 1;
		Item.consumable = true;
		Item.useStyle = ItemUseStyleID.EatFood;
		Item.useTime = 24;
		Item.useAnimation = 24;
		Item.UseSound = SoundID.Item29;
		Item.autoReuse = false;
	}

	// Blocking here (instead of failing inside UseItem) guarantees the item is never
	// consumed when the slot is already open.
	public override bool CanUseItem(Player player) => !Unlocked(player.GetModPlayer<HeartSlotsPlayer>());

	public override bool? UseItem(Player player)
	{
		HeartSlotsPlayer modPlayer = player.GetModPlayer<HeartSlotsPlayer>();

		if (Unlocked(modPlayer)) {
			return false;
		}

		Unlocked(modPlayer) = true;

		if (Main.myPlayer == player.whoAmI) {
			Main.NewText(Language.GetTextValue("Mods.HeartSlots.Misc.SlotUnlocked"), 255, 224, 128);
		}

		return true;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		HeartSlotsPlayer modPlayer = Main.LocalPlayer.GetModPlayer<HeartSlotsPlayer>();
		string key = Unlocked(modPlayer)
			? "Mods.HeartSlots.Misc.AlreadyUnlocked"
			: "Mods.HeartSlots.Misc.WillUnlock";

		tooltips.Add(new TooltipLine(Mod, "HeartSlotsState", Language.GetTextValue(key)));
	}
}
