using HarmonyLib;

namespace SuspiciousAPI.Features.Helpers.Unity.Patches;

[HarmonyPatch(typeof(UnityEngine.Object))]
public static class GameObjectDestroyPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static bool DestroyPrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object), typeof(float) })]
    public static bool TimedDestroyPrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.DestroyImmediate), new Type[] { typeof(UnityEngine.Object) })]
    public static bool DestroyImmediatePrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.DestroyImmediate), new Type[] { typeof(UnityEngine.Object), typeof(bool) })]
    public static bool ForcefulDestroyImmediatePrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.DestroyObject), new Type[] { typeof(UnityEngine.Object) })]
    public static bool DestroyObjectPrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.DestroyObject), new Type[] { typeof(UnityEngine.Object), typeof(float) })]
    public static bool TimedDestroyObjectPrefix(UnityEngine.Object obj)
    {
        return !ShouldSaveObject(obj);
    }

    /// <summary>
    /// Determines whether or not the GameObject should be saved and removes it from the list.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool ShouldSaveObject(UnityEngine.Object obj, bool removeFromList = true)
    {
        bool result = ObjectHelpers.ObjectsToBeSaved.Contains(obj);

        if (result && removeFromList)
            obj.AllowObjectDestruction();

        return result;
    }
}
