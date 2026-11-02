using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace HeartSlots.Common;

/// <summary>
/// Stores which accessory slots this character has permanently unlocked.
/// Saved with the player file, so the slots survive a restart.
/// </summary>
public class HeartSlotsPlayer : ModPlayer
{
	public bool crystalHeart;
	public bool ominousTalisman;
	public bool mechanicalExpander;
	public bool celestialSynthesizer;

	public int UnlockMask =>
		(crystalHeart ? 1 : 0) |
		(ominousTalisman ? 2 : 0) |
		(mechanicalExpander ? 4 : 0) |
		(celestialSynthesizer ? 8 : 0);

	public void ApplyMask(int mask)
	{
		crystalHeart = (mask & 1) != 0;
		ominousTalisman = (mask & 2) != 0;
		mechanicalExpander = (mask & 4) != 0;
		celestialSynthesizer = (mask & 8) != 0;
	}

	public override void SaveData(TagCompound tag)
	{
		tag["unlocks"] = UnlockMask;
	}

	public override void LoadData(TagCompound tag)
	{
		ApplyMask(tag.GetInt("unlocks"));
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		((HeartSlotsPlayer)targetCopy).ApplyMask(UnlockMask);
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		if (((HeartSlotsPlayer)clientPlayer).UnlockMask != UnlockMask) {
			SyncPlayer(toClient: -1, fromClient: Player.whoAmI, syncPlayer: true);
		}
	}

	public override void SyncPlayer(int toClient, int fromClient, bool syncPlayer)
	{
		if (Main.netMode == NetmodeID.SinglePlayer) {
			return;
		}

		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)HeartSlotsMessageType.SyncUnlocks);
		packet.Write((byte)Player.whoAmI);
		packet.Write(UnlockMask);
		packet.Send(toClient, fromClient);
	}
}
