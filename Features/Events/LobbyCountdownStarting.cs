using SuspiciousAPI.Features.Events.Core;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called only on the server right before the LobbyCountdown starts. Is Cancellable.
/// </summary>
public class LobbyCountdownStarting : EventBase
{
    public override bool Cancellable => true;

    public LobbyCountdownStarting()
    {

    }
}
