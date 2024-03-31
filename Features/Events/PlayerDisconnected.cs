using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called when a <see cref="Features.Player"/> has disconnected for any reason.
/// </summary>
public sealed class PlayerDisconnected : EventBase
{
    public Player Player { get; }
    public DisconnectReasons DisconnectReason { get; }

    public PlayerDisconnected(Player player, DisconnectReasons disconnectReason)
    {
        Player = player;
        DisconnectReason = disconnectReason;
    }
}
