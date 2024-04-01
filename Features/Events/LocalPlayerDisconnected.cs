using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called when the LocalPlayer disconnects for any reason.
/// </summary>
public class LocalPlayerDisconnected : PlayerDisconnected
{
    // FIXME: This should be a seperate class that does not return PlayerControl, since it may be null.
    public LocalPlayerDisconnected(Player player, DisconnectReasons disconnectReason) : base(player, disconnectReason)
    {
    }
}