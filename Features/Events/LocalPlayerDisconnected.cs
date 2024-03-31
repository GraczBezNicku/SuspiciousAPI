using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events;

public class LocalPlayerDisconnected : PlayerDisconnected
{
    public LocalPlayerDisconnected(Player player, DisconnectReasons disconnectReason) : base(player, disconnectReason)
    {
    }
}