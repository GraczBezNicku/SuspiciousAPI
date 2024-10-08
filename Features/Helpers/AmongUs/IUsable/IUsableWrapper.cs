﻿using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppSystem.Reflection;
using SuspiciousAPI.Features.Helpers.IL2CPP;
using SuspiciousAPI.Features.Interactables.Patches;
using System.Collections;
using UnityEngine;

namespace SuspiciousAPI.Features.Helpers.AmongUs.IUsable;

public class IUsableWrapper
{
    public static readonly Type[] AllBaseGameImplementers =
    {
        typeof(Console),
        typeof(DeconControl),
        typeof(DoorConsole),
        typeof(MapConsole),
        typeof(OpenDoorConsole),
        typeof(OptionsConsole),
        typeof(PlatformConsole),
        typeof(SystemConsole),
        typeof(Vent),
        typeof(Ladder),
        typeof(ZiplineConsole)
    };

    private static readonly string[] _IUsablePatchTargets =
    {
        "M_SetOutline",
        "M_CanUse",
        "M_Use",
    };

    private static readonly string[] _IUsableCoolDownPatchTargets =
    {
        "M_IsCoolingDown",
    };

    public static List<Type> KnownIUsableImplementers = new List<Type>();
    public static List<Type> KnownIUsableCooldownImplementers = new List<Type>();

    private static List<Type> IUsableBlacklistedTypes = new List<Type>();
    private static List<Type> IUsableCooldownBlacklistedTypes = new List<Type>();

    /// <summary>
    /// Registers all IUsable / IUsableCoolDown implementers in the provided <see cref="Type"/> array.
    /// </summary>
    /// <param name="assembly"></param>
    public static void RegisterUsables(Type[] types)
    {
        foreach (Type type in types)
        {
            ImplementsIUsableCoolDown(Il2CppType.From(type));
        }

        if (!types.Any(x => KnownIUsableCooldownImplementers.Contains(x) || KnownIUsableImplementers.Contains(x)))
            return;

        Type[] IUsableImplementers = KnownIUsableImplementers.Where(x => types.Contains(x)).ToArray();

        IEnumerator PatchAllTargets(Type[] types, string[] targets)
        {
            foreach (Type type in types)
            {
                yield return new WaitForEndOfFrame();

                foreach (string patchTarget in targets)
                {
                    yield return new WaitForEndOfFrame();

                    try
                    {
                        Logger.LogMessage($"Beginning patches for {type.Name} ({patchTarget})");
                        bool isProperty = patchTarget.StartsWith("P_");
                        string trueName = patchTarget.Substring(2, patchTarget.Length - 2);

                        System.Reflection.PropertyInfo _backingProperty = type.GetProperty(trueName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                        System.Reflection.MethodBase target = isProperty
                            ? (_backingProperty == null ? null : _backingProperty.GetGetMethod())
                            : type.GetMethod(trueName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                        if (target == null)
                        {
                            Logger.LogError($"A presumed IUsable implementer lacks an IUsable element ({trueName})");
                            continue;
                        }

                        var patches = Harmony.GetPatchInfo(target);

                        if (patches != null && patches.Owners.Any(x => x == BepInExPlugin.Instance._harmony.Id))
                        {
                            Logger.LogMessage($"This method was already patched by SusAPI. Skipping...");
                            continue;
                        }

                        var patchMethod = typeof(IUsablePatches).GetMethod($"{trueName}Prefix", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                        if (patchMethod == null)
                        {
                            Logger.LogError($"Tried patching with a method that doesn't exist! ({trueName}Prefix)");
                            continue;
                        }

                        Logger.LogMessage($"Patching {patchTarget} for {type.Name}");

                        PatchProcessor proc = BepInExPlugin.Instance._harmony.CreateProcessor(target);
                        proc.AddPrefix(patchMethod);
                        proc.Patch();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Failed patching for a specified patch target!\n{ex.Message}\n{ex.StackTrace}\n{ex.Source}\n{ex.InnerException}");
                        continue;
                    }
                }
            }
        }

        List<string> allTargets = new List<string>();
        foreach (string target in _IUsablePatchTargets)
            allTargets.Add(target);
        foreach (string target in _IUsableCoolDownPatchTargets)
            allTargets.Add(target);

        CoroutineHelper.Instance.StartCoroutine(PatchAllTargets(IUsableImplementers, allTargets.ToArray()));
    }

    /// <summary>
    /// Checks if the provided type implements IUsable. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsable, <see langword="false"/> if not.</returns>
    private static bool ImplementsIUsable(Il2CppSystem.Type type)
    {
        if (KnownIUsableImplementers.Contains(type.GetManagedType()))
            return true;

        if (IUsableBlacklistedTypes.Contains(type.GetManagedType()))
            return false;

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        if (BepInExConfig.DebugMode)
        {
            FieldInfo[] publicFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo pubField in publicFields)
            {
                Logger.LogDebug($"{pubField.Name} (public field)", BepInExConfig.DebugMode);
            }

            foreach (FieldInfo privField in privateFields)
            {
                Logger.LogDebug($"{privField.Name} (private field)", BepInExConfig.DebugMode);
            }
        }

        int foundElements = 0;

        foreach (MethodInfo method in methods)
        {
            Logger.LogDebug($"{method.Name} (method; ReturnType: {method.ReturnType.Name})", BepInExConfig.DebugMode);
            if (method.Name == "SetOutline" && method.ReturnType == Il2CppType.From(typeof(void)))
                foundElements++;

            if (method.Name == "CanUse" && method.ReturnType == Il2CppType.From(typeof(float)))
                foundElements++;

            if (method.Name == "Use" && method.ReturnType == Il2CppType.From(typeof(void)))
                foundElements++;
        }

        foreach (PropertyInfo property in properties)
        {
            Logger.LogDebug($"{property.Name} (property; PropertyType: {property.PropertyType.Name})", BepInExConfig.DebugMode);
            if (property.Name == "UsableDistance" && property.PropertyType == Il2CppType.From(typeof(float)))
                foundElements++;

            if (property.Name == "PercentCool" && property.PropertyType == Il2CppType.From(typeof(float)))
                foundElements++;

            if (property.Name == "UseIcon" && property.PropertyType == Il2CppType.From(typeof(ImageNames)))
                foundElements++;
        }

        Logger.LogMessage($"Found elements for type {type.Name}: {foundElements}");

        if (foundElements < 6 && !IUsableBlacklistedTypes.Contains(type.GetManagedType()))
        {
            IUsableBlacklistedTypes.Add(type.GetManagedType());
        }

        if (foundElements >= 6 && !KnownIUsableImplementers.Contains(type.GetManagedType()))
        {
            KnownIUsableImplementers.Add(type.GetManagedType());
        }

        return foundElements >= 6;
    }

    /// <summary>
    /// Checks if the provided type implements IUsableCoolDown. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsableCoolDown, <see langword="false"/> if not.</returns>
    private static bool ImplementsIUsableCoolDown(Il2CppSystem.Type type)
    {
        if (KnownIUsableCooldownImplementers.Contains(type.GetManagedType()))
            return true;

        if (IUsableCooldownBlacklistedTypes.Contains(type.GetManagedType()))
            return false;

        if (!ImplementsIUsable(type))
            return false;

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        int foundElements = 0;

        foreach (MethodInfo method in methods)
        {
            Logger.LogDebug($"{method.Name} (method)", BepInExConfig.DebugMode);
            if (method.Name == "IsCoolingDown" && method.ReturnType.Name == "bool")
                foundElements++;
        }

        foreach (PropertyInfo property in properties)
        {
            Logger.LogDebug($"{property.Name} (property)", BepInExConfig.DebugMode);
            if (property.Name == "CoolDown" && property.PropertyType.Name == "float")
                foundElements++;

            if (property.Name == "MaxCoolDown" && property.PropertyType.Name == "float")
                foundElements++;
        }

        if (foundElements < 3 && !IUsableCooldownBlacklistedTypes.Contains(type.GetManagedType()))
        {
            IUsableCooldownBlacklistedTypes.Add(type.GetManagedType());
        }

        if (foundElements >= 3 && !KnownIUsableCooldownImplementers.Contains(type.GetManagedType()))
        {
            KnownIUsableCooldownImplementers.Add(type.GetManagedType());
        }

        return foundElements >= 3;
    }
}
