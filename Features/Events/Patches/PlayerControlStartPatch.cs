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

    // FIXME: This works only on the LocalPlayer. You'll need to find a better entry point to inject this into, since this can cause a nullref
    public static IEnumerator InitializeAfterOneFrame(PlayerControl pc)
    {
        yield return new WaitForEndOfFrame();
        Logger.LogDebug($"Finished enumerating PlayerControl::Start(), running patch!", BepInExConfig.DebugMode);
        EventManager.ExecuteEvent(new PlayerInitialized(Player.Get(pc)));
    }
}
