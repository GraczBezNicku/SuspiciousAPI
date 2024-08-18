using SuspiciousAPI.Features.Events.Core;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called after a <see cref="PlayerControl"/> finishes initializing.
/// </summary>
public sealed class PlayerInitialized : EventBase
{
    public Player Player { get; }

    public PlayerInitialized(Player player)
    {
        Player = player;
    }
}
