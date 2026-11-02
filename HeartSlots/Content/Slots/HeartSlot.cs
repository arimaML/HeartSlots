using HeartSlots.Common;
using Terraria;
using Terraria.ModLoader;

namespace HeartSlots.Content.Slots;

/// <summary>
/// One extra accessory slot, unlocked by a single item.
/// Functional cell only: giving every slot a vanity cell would turn 4 slots into 8 cells
/// and push the column off screen at high UI scale.
/// </summary>
public abstract class HeartSlot : ModAccessorySlot
{
	protected abstract ref bool Unlocked(HeartSlotsPlayer modPlayer);

	// Show only the functional cell. A default slot also draws vanity and dye cells,
	// which would turn 4 slots into 12 cells and push the column off screen at high UI scale.
	public override bool DrawFunctionalSlot => true;

	public override bool DrawVanitySlot => false;

	public override bool DrawDyeSlot => false;

	public override bool IsEnabled()
	{
		Terraria.Player currentPlayer = ModAccessorySlot.Player;

		if (currentPlayer is null || !currentPlayer.active) {
			return false;
		}

		return Unlocked(currentPlayer.GetModPlayer<HeartSlotsPlayer>());
	}
}
