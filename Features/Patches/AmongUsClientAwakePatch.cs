using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SuspiciousAPI.Features.Helpers.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Patches;

[HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
public static class AmongUsClientAwakePatch
{
    public static void Prefix()
    {
        if (!UnityMethods.Instance)
        {
            GameObject obj = new GameObject("UnityMethods");
            UnityEngine.Object.DontDestroyOnLoad(obj);

            UnityMethods comp = obj.AddComponent<UnityMethods>();
            UnityMethods.Instance = comp;
        }
    }
}
