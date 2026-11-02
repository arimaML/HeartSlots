using HeartSlots.Common;

namespace HeartSlots.Content.Slots;

public class CrystalSlot : HeartSlot
{
	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.crystalHeart;
}
