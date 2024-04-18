using SuspiciousAPI.Features.Interactables.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Interactables.Patches;

// TODO: Implement all patches
public static class IUsablePatches
{
    public static bool UsePrefix(object __instance)
    {
        //Logger.LogMessage($"TESTING! TESTING! USE HAS BEEN USED! ({(__instance as UnityEngine.Component).gameObject.name})");
        return true;
    }

    public static bool UseIconPrefix(object __instance, ref ImageNames __result)
    {
        Interactable interactable = Interactable.Get(__instance as UnityEngine.Component);

        if (interactable == null)
            return true;

        __result = interactable.UseIcon;
        return false;
    }
}
