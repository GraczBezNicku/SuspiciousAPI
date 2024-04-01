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
    public static void Prefix(PlayerControl __instance)
    {
        CoroutineHelper.Instance.StartCoroutine(CallEventOnInitialization(__instance));
    }

    public static IEnumerator CallEventOnInitialization(PlayerControl ctrl)
    {
        int timeoutFrames = 600;
        while (timeoutFrames > 0)
        {
            yield return new WaitForEndOfFrame();

            if (ctrl.CurrentOutfit == null)
            {
                timeoutFrames--;
                continue;
            }

            if (ctrl.CurrentOutfit.IsIncomplete)
            {
                timeoutFrames--;
                continue;
            }

            EventManager.ExecuteEvent(new PlayerInitialized(Player.Get(ctrl)));
            break;
        }
    }
}
