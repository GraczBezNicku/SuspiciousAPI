using HarmonyLib;
using Hazel;
using SuspiciousAPI.Features.CustomRPCs.Core;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
public static class EventsHandleRpcPatch
{
    public static void Prefix(byte callId, MessageReader reader)
    {
        byte trueId = RpcManager.GetTrueRpcId(callId);
        MessageReader copy = MessageReader.Get(reader);

        if (trueId != (byte)RpcCalls.SetStartCounter)
            return;

        int sequenceId = copy.ReadPackedInt32();
        sbyte timer = copy.ReadSByte();

        if (timer != 5)
            return;

        EventManager.ExecuteEvent(new LobbyCountdownStarted());
    }
}
