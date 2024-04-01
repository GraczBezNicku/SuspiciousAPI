using HarmonyLib;
using InnerNet;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.DisconnectInternal))]
public static class DisconnectInternalPatch
{
    public static void Postfix(DisconnectReasons reason, [Optional] string stringReason)
    {
        EventManager.ExecuteEvent(new LocalPlayerDisconnected(Player.LocalPlayer, reason));
    }
}
