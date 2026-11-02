using HeartSlots.Common;
using Terraria.ID;

namespace HeartSlots.Content.Items;

public class MechanicalExpander : SlotUnlockItem
{
	public override string Texture => "HeartSlots/Assets/Items/MechanicalExpander";

	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.mechanicalExpander;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.rare = ItemRarityID.Pink;
		Item.value = Terraria.Item.sellPrice(gold: 1);
	}
}
