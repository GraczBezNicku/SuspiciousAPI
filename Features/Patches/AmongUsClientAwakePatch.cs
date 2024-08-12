using HarmonyLib;
using SuspiciousAPI.Features.Helpers.AmongUs.IUsable;
using SuspiciousAPI.Features.Helpers.Unity;
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

            // Handle IUsables here
            IUsableWrapper.RegisterUsables(IUsableWrapper.AllBaseGameImplementers);
        }
    }
}
