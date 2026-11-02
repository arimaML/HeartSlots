using HeartSlots.Common;

namespace HeartSlots.Content.Slots;

public class CelestialSlot : HeartSlot
{
	protected override ref bool Unlocked(HeartSlotsPlayer modPlayer) => ref modPlayer.celestialSynthesizer;
}
