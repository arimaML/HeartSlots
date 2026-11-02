using HeartSlots.Common;

namespace HeartSlots.Content.Slots;

public class OminousSlot : HeartSlot
{
	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.ominousTalisman;
}
