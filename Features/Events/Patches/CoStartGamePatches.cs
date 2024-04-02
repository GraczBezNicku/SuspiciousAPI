using HarmonyLib;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(AmongUsClient))]
public static class CoStartGamePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGameHost))]
    public static void CoStartGameHostPostfix()
    {
        EventManager.ExecuteEvent(new LobbyRoundStarted());
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGameClient))]
    public static void CoStartGameClientPostfix()
    {
        EventManager.ExecuteEvent(new LobbyRoundStarted());
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CoStartGame))]
    public static void CoStartGamePrefix()
    {
        if (Lobby.AmHost)
            return;

        EventManager.ExecuteEvent(new LobbyRoundStarting());
    }
}
