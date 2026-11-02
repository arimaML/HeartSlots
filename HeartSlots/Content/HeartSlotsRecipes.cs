using HeartSlots.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HeartSlots.Content;

/// <summary>
/// All crafting content lives here: three "any of these" recipe groups plus four altar recipes.
/// Standard Recipe.Create registration means Recipe Browser picks these up with no extra work.
/// </summary>
public class HeartSlotsRecipes : ModSystem
{
	public override void AddRecipeGroups()
	{
		// Demon worlds drop Shadow Scales + Demonite, crimson worlds drop Tissue Samples + Crimtane.
		RegisterGroup("HeartSlots:EvilScale", ItemID.ShadowScale, ItemID.TissueSample);
		RegisterGroup("HeartSlots:EvilBar", ItemID.DemoniteBar, ItemID.CrimtaneBar);
		RegisterGroup("HeartSlots:Anvil", ItemID.IronAnvil, ItemID.LeadAnvil);
		RegisterGroup("HeartSlots:MechSummon", ItemID.MechanicalEye, ItemID.MechanicalWorm, ItemID.MechanicalSkull);
	}

	private static void RegisterGroup(string key, params int[] items)
	{
		// Evaluated lazily so the localized "Any X" label is built after localization loads.
		var group = new RecipeGroup(
			() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(items[0]),
			items);

		RecipeGroup.RegisterGroup(key, group);
	}

	public override void AddRecipes()
	{
		Recipe crystal = Recipe.Create(ModContent.ItemType<CrystalHeart>());
		crystal.AddIngredient(ItemID.LifeCrystal, 10);
		crystal.AddIngredient(ItemID.Diamond, 10);
		crystal.AddIngredient(ItemID.Sapphire, 10);
		crystal.AddIngredient(ItemID.Amethyst, 10);
		crystal.AddTile(TileID.DemonAltar);
		crystal.Register();

		Recipe talisman = Recipe.Create(ModContent.ItemType<OminousTalisman>());
		talisman.AddRecipeGroup("HeartSlots:EvilScale", 10);
		talisman.AddRecipeGroup("HeartSlots:Anvil", 1);
		talisman.AddIngredient(ItemID.Bone, 50);
		talisman.AddRecipeGroup("HeartSlots:EvilBar", 10);
		talisman.AddTile(TileID.DemonAltar);
		talisman.Register();

		Recipe expander = Recipe.Create(ModContent.ItemType<MechanicalExpander>());
		expander.AddIngredient(ItemID.ChlorophyteBar, 15);
		expander.AddIngredient(ItemID.HallowedBar, 15);
		expander.AddIngredient(ItemID.SoulofLight, 5);
		expander.AddIngredient(ItemID.SoulofNight, 5);
		expander.AddRecipeGroup("HeartSlots:MechSummon", 1);
		expander.AddTile(TileID.DemonAltar);
		expander.Register();

		Recipe synthesizer = Recipe.Create(ModContent.ItemType<CelestialSynthesizer>());
		synthesizer.AddIngredient(ItemID.LunarBar, 20);
		synthesizer.AddIngredient(ItemID.FragmentSolar, 10);
		synthesizer.AddIngredient(ItemID.FragmentVortex, 10);
		synthesizer.AddIngredient(ItemID.FragmentNebula, 10);
		synthesizer.AddIngredient(ItemID.FragmentStardust, 10);
		synthesizer.AddTile(TileID.DemonAltar);
		synthesizer.Register();
	}
}
