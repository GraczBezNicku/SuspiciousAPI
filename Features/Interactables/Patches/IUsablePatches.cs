namespace SuspiciousAPI.Features.Interactables.Patches;

// TODO: Implement all patches
/// <summary>
/// This class contains all patches for IUsable objects.
/// </summary>
public static class IUsablePatches
{
    public static bool UsePrefix(object __instance)
    {
        Logger.LogMessage($"TESTING! TESTING! USE HAS BEEN USED! ({(__instance as UnityEngine.Component).gameObject.name})");
        return true;
    }
}
