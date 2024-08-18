using SuspiciousAPI.Features.Events.Core;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called when a <see cref="Features.Player"/> has disconnected for any reason. NOTE: This does not apply to the LocalPlayer.
/// </summary>
public class PlayerDisconnected : EventBase
{
    public Player Player { get; }
    public DisconnectReasons DisconnectReason { get; }

    public PlayerDisconnected(Player player, DisconnectReasons disconnectReason)
    {
        Player = player;
        DisconnectReason = disconnectReason;
    }
}
