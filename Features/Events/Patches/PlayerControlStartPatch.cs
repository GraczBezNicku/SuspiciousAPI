using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
public static class PlayerControlStartPatch
{
    public static void Postfix(PlayerControl __instance)
    {
        CoroutineHelper.Instance.StartCoroutine(InitializeAfterOneFrame(__instance));
    }

    // As of now, I don't have any better ideas on how to handle this. One frame difference is still good, considering that anything under one frame would require a manual patch by a plugin dev.
    public static IEnumerator InitializeAfterOneFrame(PlayerControl pc)
    {
        yield return new WaitForEndOfFrame();
        Logger.LogDebug($"Finished enumerating PlayerControl::Start(), running patch!", BepInExConfig.DebugMode);
        EventManager.ExecuteEvent(new PlayerInitialized(Player.Get(pc)));
    }
}
