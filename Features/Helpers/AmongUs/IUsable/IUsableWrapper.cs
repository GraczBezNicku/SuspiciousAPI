using Il2CppInterop.Runtime;
using Il2CppSystem.Reflection;

namespace SuspiciousAPI.Features.Helpers.AmongUs.IUsable;

public class IUsableWrapper
{ 
    // FIXME: GetType() does not resolve the base Component inherited classes. Find a different way to access a component.
    /// <summary>
    /// Checks if the provided type implements IUsable. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsable, <see langword="false"/> if not.</returns>
    public static bool ImplementsIUsable(Il2CppSystem.Type type)
    {
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
        return foundElements >= 6;
    }

    /// <summary>
    /// Checks if the provided type implements IUsableCoolDown. Technically it can be tricked by using override methods, but the base game doesn't do that.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the type implements IUsableCoolDown, <see langword="false"/> if not.</returns>
    public static bool ImplementsIUsableCoolDown(Il2CppSystem.Type type)
    {
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

        return foundElements >= 3;
    }
}
