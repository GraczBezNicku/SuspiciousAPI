using HarmonyLib;
using InnerNet;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.CreatePlayer))]
public static class CreatePlayerPatch
{
    public static void Prefix(ClientData clientData)
    {
        EventManager.ExecuteEvent(new PlayerJoining(clientData));
    }
}
