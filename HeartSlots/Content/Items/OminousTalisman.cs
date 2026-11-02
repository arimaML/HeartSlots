using HeartSlots.Common;
using Terraria.ID;

namespace HeartSlots.Content.Items;

public class OminousTalisman : SlotUnlockItem
{
	public override string Texture => "HeartSlots/Assets/Items/OminousTalisman";

	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.ominousTalisman;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.rare = ItemRarityID.Green;
		Item.value = Terraria.Item.sellPrice(gold: 25);
	}
}
