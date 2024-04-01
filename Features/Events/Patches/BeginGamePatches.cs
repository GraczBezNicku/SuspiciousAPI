using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SuspiciousAPI.Features.Events.Patches;

/*
// I'm gonna keep this in case I need it, but for now there may be a few places where the lobbycountdownstarted event can go...
[HarmonyPatch(typeof(GameStartManager))]
public static class BeginGamePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
    public static void BeginGamePatch(GameStartManager __instance)
    {
        if (__instance.startState != GameStartManager.StartingStates.Countdown)
            return;

        CoroutineHelper.Instance.StartCoroutine(PrintAfterTenFrames());

        //EventManager.ExecuteEvent(new LobbyCountdownStarted(__instance));
    }

    public static IEnumerator PrintAfterTenFrames()
    {
        for (int i = 0; i <= 10; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        TextMeshPro[] texts = UnityEngine.Object.FindObjectsOfType<TextMeshPro>();

        Logger.LogMessage($"Any texts that matches starting: {texts.Any(x => x.text.Contains("Starting"))}");

        foreach (TextMeshPro text in texts)
        {
            Logger.LogError(text.text);

            if (text.text.Contains("Starting"))
            {
                Logger.LogMessage($"Starting TEXT:\n{text.gameObject.name}\n");
                foreach (Component comp in text.gameObject.GetComponents<Component>())
                {
                    Logger.LogMessage($"Found comp: {comp.GetType()}\n");
                }
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.ReallyBegin))]
    public static void ReallyBeginPatch(GameStartManager __instance)
    {
        CoroutineHelper.Instance.StartCoroutine(PrintAfterTenFrames());
        //EventManager.ExecuteEvent(new LobbyCountdownStarted(__instance));
    }
}
*/
