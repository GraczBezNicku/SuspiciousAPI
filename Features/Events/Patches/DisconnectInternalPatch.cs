using HarmonyLib;
using InnerNet;
using SuspiciousAPI.Features.Events.Core;
using System.Runtime.InteropServices;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.DisconnectInternal))]
public static class DisconnectInternalPatch
{
    public static void Postfix(DisconnectReasons reason, [Optional] string stringReason)
    {
        EventManager.ExecuteEvent(new LocalPlayerDisconnected(Player.LocalPlayer, reason));
    }
}
