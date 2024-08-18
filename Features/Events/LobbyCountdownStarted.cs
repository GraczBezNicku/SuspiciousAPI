using SuspiciousAPI.Features.Events.Core;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called on the server as soon as the countdown is started. Called for clients when they receive the LobbyCountdown RPC.
/// </summary>
public class LobbyCountdownStarted : EventBase
{
    public LobbyCountdownStarted()
    {

    }
}
