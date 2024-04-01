using InnerNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features;

/// <summary>
/// Various utilities for <see cref="Lobby"/> functionality. Most of them require the LocalPlayer to be the host.
/// </summary>
public class Lobby
{
    public static bool AmHost => AmongUsClient.Instance.AmHost;

    public static void StopLobbyCountdown()
    {

    }
}
