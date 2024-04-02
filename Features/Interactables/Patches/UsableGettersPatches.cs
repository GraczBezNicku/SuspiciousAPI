using HarmonyLib;
using SuspiciousAPI.Features.Interactables.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Interactables.Patches;

[HarmonyPatch(typeof(IUsable))]
public static class UsableGettersPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(IUsable), nameof(IUsable.UsableDistance), MethodType.Getter)]
    public static bool UsableDistancePrefix(ref float __result)
    {
        Interactable interactable = 
    }
}