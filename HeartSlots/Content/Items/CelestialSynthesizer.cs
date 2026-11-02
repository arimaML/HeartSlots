using HeartSlots.Common;
using Terraria.ID;

namespace HeartSlots.Content.Items;

public class CelestialSynthesizer : SlotUnlockItem
{
	public override string Texture => "HeartSlots/Assets/Items/CelestialSynthesizer";

	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.celestialSynthesizer;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.rare = ItemRarityID.Purple;
		Item.value = Terraria.Item.sellPrice(platinum: 1);
	}
}
