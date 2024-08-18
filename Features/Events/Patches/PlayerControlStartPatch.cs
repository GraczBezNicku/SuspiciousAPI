using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using SuspiciousAPI.Features.Events.Core;
using System.Collections;
using UnityEngine;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
public static class PlayerControlStartPatch
{
    public static void Prefix(PlayerControl __instance)
    {
        CoroutineHelper.Instance.StartCoroutine(CallEventOnInitialization(__instance));
    }

    public static IEnumerator CallEventOnInitialization(PlayerControl ctrl)
    {
        while (ctrl != null)
        {
            yield return new WaitForEndOfFrame();

            if (ctrl.CurrentOutfit == null)
                continue;

            if (ctrl.CurrentOutfit.IsIncomplete)
                continue;

            EventManager.ExecuteEvent(new PlayerInitialized(Player.Get(ctrl)));
            break;
        }
    }
}
