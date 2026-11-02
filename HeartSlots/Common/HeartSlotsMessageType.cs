namespace HeartSlots.Common;

/// <summary>
/// Network message identifiers for this mod. Every message starts with one of these bytes.
/// </summary>
public enum HeartSlotsMessageType : byte
{
	SyncUnlocks = 1
}
