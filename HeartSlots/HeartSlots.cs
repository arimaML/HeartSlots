using System.IO;
using HeartSlots.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HeartSlots;

public class HeartSlots : Mod
{
	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		var type = (HeartSlotsMessageType)reader.ReadByte();

		if (type != HeartSlotsMessageType.SyncUnlocks) {
			Logger.Warn($"HeartSlots: unknown message type {(byte)type}");
			return;
		}

		int playerIndex = reader.ReadByte();
		int mask = reader.ReadInt32();

		// Server relays the update so every other client stays in sync.
		if (Main.netMode == NetmodeID.Server) {
			ModPacket relay = GetPacket();
			relay.Write((byte)HeartSlotsMessageType.SyncUnlocks);
			relay.Write((byte)playerIndex);
			relay.Write(mask);
			relay.Send(ignoreClient: whoAmI);
		}

		if (playerIndex >= 0 && playerIndex < Main.maxPlayers) {
			Player player = Main.player[playerIndex];
			if (player != null && player.active) {
				player.GetModPlayer<HeartSlotsPlayer>().ApplyMask(mask);
			}
		}
	}
}
