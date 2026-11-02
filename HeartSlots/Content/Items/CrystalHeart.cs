using HeartSlots.Common;
using Terraria.ID;

namespace HeartSlots.Content.Items;

public class CrystalHeart : SlotUnlockItem
{
	public override string Texture => "HeartSlots/Assets/Items/CrystalHeart";

	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.crystalHeart;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.rare = ItemRarityID.Blue;
		Item.value = Terraria.Item.sellPrice(gold: 10);
	}
}
