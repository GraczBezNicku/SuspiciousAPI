namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called when the LocalPlayer disconnects for any reason.
/// </summary>
public class LocalPlayerDisconnected : PlayerDisconnected
{
    // FIXME: In the DisconnectInternal patch we need to determine the difference when a player disconnects from a LOBBY and the SERVER BROWSER
    public LocalPlayerDisconnected(Player player, DisconnectReasons disconnectReason) : base(player, disconnectReason)
    {
    }
}