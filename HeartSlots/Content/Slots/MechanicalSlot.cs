using HeartSlots.Common;

namespace HeartSlots.Content.Slots;

public class MechanicalSlot : HeartSlot
{
	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.mechanicalExpander;
}
