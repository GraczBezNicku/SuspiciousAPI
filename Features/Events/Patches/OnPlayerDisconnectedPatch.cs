using HarmonyLib;
using InnerNet;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerLeft))]
public static class OnPlayerDisconnectedPatch
{
    public static void Prefix(ClientData data, DisconnectReasons reason)
    {
        EventManager.ExecuteEvent(new PlayerDisconnected(Player.Get(data), reason));
    }
}
