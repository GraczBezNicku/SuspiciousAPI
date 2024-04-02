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

[HarmonyPatch(typeof(GameStartManager))]
public static class BeginGamePatches
{
    public static bool LastReturnValue = false;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
    public static bool BeginGamePrefix(GameStartManager __instance)
    {
        LastReturnValue = true;

        if (__instance.startState != GameStartManager.StartingStates.NotStarting)
            return true;

        if (GameData.Instance.AllPlayers._size < __instance.MinPlayers)
            return true;

        if (!EventManager.ExecuteEvent(new LobbyCountdownStarting()))
        {
            LastReturnValue = false;
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.ReallyBegin))]
    public static bool ReallyBeginPrefix(GameStartManager __instance)
    {
        if (!EventManager.ExecuteEvent(new LobbyCountdownStarting()))
        {
            LastReturnValue = false;
            return false;
        }

        LastReturnValue = true;
        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
    public static void BeginGamePostfix(GameStartManager __instance)
    {
        if (!LastReturnValue)
            return;

        if (__instance.startState != GameStartManager.StartingStates.Countdown)
            return;

        EventManager.ExecuteEvent(new LobbyCountdownStarted());
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.ReallyBegin))]
    public static void ReallyBeginPostfix(GameStartManager __instance)
    {
        if (!LastReturnValue)
            return;

        EventManager.ExecuteEvent(new LobbyCountdownStarted());
    }
}
