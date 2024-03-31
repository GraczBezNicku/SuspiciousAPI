using InnerNet;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events;

/// <summary>
/// Called before any preparations for creating a <see cref="PlayerControl"/> object takes place. It's recomenned to not use <see cref="Player"/> unless absolutely necessary.
/// </summary>
public class PlayerJoining : EventBase
{
    public ClientData PlayerData { get; }

    public PlayerJoining(ClientData playerData)
    {
        PlayerData = playerData;
    }
}