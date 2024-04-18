using Il2CppInterop.Runtime;
using Il2CppSystem.Reflection;
using System.Collections.Generic;

namespace SuspiciousAPI.Features.Helpers.AmongUs.IUsable;

public class IUsableWrapper
{
    private static List<Il2CppSystem.Type> KnownIUsableImplementers = new List<Il2CppSystem.Type>();
    private static List<Il2CppSystem.Type> KnownIUsableCooldownImplementers = new List<Il2CppSystem.Type>();

    private static List<Il2CppSystem.Type> IUsableBlacklistedTypes = new List<Il2CppSystem.Type>();
    private static List<Il2CppSystem.Type> IUsableCooldownBlacklistedTypes = new List<Il2CppSystem.Type>();

    /// <summary>
    /// Checks if the provided type implements IUsable. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsable, <see langword="false"/> if not.</returns>
    public static bool ImplementsIUsable(Il2CppSystem.Type type)
    {
        if (KnownIUsableImplementers.Contains(type))
            return true;

        if (IUsableBlacklistedTypes.Contains(type))
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

        if (foundElements < 6 && !IUsableBlacklistedTypes.Contains(type))
        {
            IUsableBlacklistedTypes.Add(type);
        }

        if (foundElements >= 6 && !KnownIUsableImplementers.Contains(type))
        {
            KnownIUsableImplementers.Add(type);
        }

        return foundElements >= 6;
    }

    /// <summary>
    /// Checks if the provided type implements IUsableCoolDown. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsableCoolDown, <see langword="false"/> if not.</returns>
    public static bool ImplementsIUsableCoolDown(Il2CppSystem.Type type)
    {
        if (KnownIUsableCooldownImplementers.Contains(type))
            return true;

        if (IUsableCooldownBlacklistedTypes.Contains(type))
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

        if (foundElements < 3 && !IUsableCooldownBlacklistedTypes.Contains(type))
        {
            IUsableCooldownBlacklistedTypes.Add(type);
        }

        if (foundElements >= 3 && !KnownIUsableCooldownImplementers.Contains(type))
        {
            KnownIUsableCooldownImplementers.Add(type);
        }

        return foundElements >= 3;
    }
}
