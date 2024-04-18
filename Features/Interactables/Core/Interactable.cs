using HarmonyLib;
using Il2CppInterop.Runtime;
using SuspiciousAPI.Features.Helpers.AmongUs.IUsable;
using SuspiciousAPI.Features.Interactables.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Interactables.Core;

/// <summary>
/// Any object in the game that the <see cref="Player"/> can interact with.
/// </summary>
public class Interactable
{
    public static Dictionary<GameObject, Interactable> GameObjectToInteractable = new Dictionary<GameObject, Interactable>();

    /// <summary>
    /// This constructor is meant to be used only for registering base-game elements, use it with caution! (If you want to create a custom interactable, use Interactable.Create())
    /// </summary>
    /// <param name="usable"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public Interactable(object usable)
    {
        if (usable is not Il2CppSystem.Object obj)
            throw new InvalidOperationException("Provided usable must exist in the IL2CPP runtime! Use Components!");

        if (!IUsableWrapper.ImplementsIUsable(obj.GetIl2CppType()))
            throw new InvalidOperationException("Usable object must implement IUsable interface!");

        Usable = usable;
        PreparePatches();
    }

    /// <summary>
    /// Prepares the freshly created Interactable object to be compatible. This method assumes that the provided <see cref="Usable"/> is a valid IUsable.
    /// </summary>
    private void PreparePatches()
    {
        // FIXME: CHECK THIS!
        Logger.LogMessage($"Starting patches for {(Usable as Component).gameObject.name}!");
        string[] targetMethods =
        {
            "SetOutline",
            "CanUse",
            "Use"
        };

        string[] targetProperties =
        {
            "UsableDistance",
            "PercentCool",
            "UseIcon"
        };

        // We don't want to patch a method that's already patched, since patches are "global" so to speak. We do this dynamically for Custom Interactables to work.
        foreach (string methodName in targetMethods)
        {
            var method = UsableScriptType.GetMethod(methodName);

            if (method == null)
            {
                Logger.LogError($"A pressumed IUsable script lacks an IUsable method! ({methodName})");
                continue;
            }

            var patches = Harmony.GetPatchInfo(method);

            if (patches.Owners.Any(x => x == BepInExPlugin.Instance._harmony.Id))
            {
                Logger.LogDebug($"This method seems to have already been patched by SusAPI. Skipping...", BepInExConfig.DebugMode);
                continue;
            }

            var patchMethod = typeof(IUsablePatches).GetMethod($"{method.Name}Prefix");

            if (patchMethod == null)
            {
                Logger.LogError($"Tried patching a method that doesn't exist! ({method.Name}Prefix)");
                continue;
            }

            BepInExPlugin.Instance._harmony.Patch(method, new HarmonyMethod(patchMethod));
        }
    }

    /// <summary>
    /// Returns the original script's type. Used in conjunction with <see cref="Usable"/>
    /// </summary>
    public Type UsableScriptType
    {
        get
        {
            return _usableScriptType;
        }
    }
    private Type _usableScriptType;

    /// <summary>
    /// Returns the original script belonging to said Usable. You'll need to cast it to the correct type using the <see cref="UsableScriptType"/> property. 
    /// </summary>
    public object Usable
    {
        get => _usable;
        set
        {
            _usable = value;
            _usableScriptType = value.GetType();
            //usableOverrides = new IUsableOverrides();
        }
    }
    private object _usable;

    /*
    public float MaxDistance
    {
        get => usableOverrides.UsableDistance;
        set => usableOverrides.UsableDistance = value;
    }
    */

    /// <summary>
    /// Gets the FIRST <see cref="Interactable"> object present on a <see cref="GameObject"/>.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns><see cref="Interactable"> if found, <see langword="null"/> if not.</see></returns>
    public static Interactable Get(GameObject gameObject)
    {
        if (GameObjectToInteractable.ContainsKey(gameObject))
            return GameObjectToInteractable[gameObject];

        Component validInteractable = null;
        Interactable interactable = null;

        Component[] components = gameObject.GetComponents<Component>();

        if (components.Any(x => IUsableWrapper.ImplementsIUsable(x.GetIl2CppType())))
        {
            validInteractable = components.First(x => IUsableWrapper.ImplementsIUsable(x.GetIl2CppType()));
            interactable = new Interactable(validInteractable);
        }
        else if (components.Any(x => IUsableWrapper.ImplementsIUsableCoolDown(x.GetIl2CppType())))
        {
            validInteractable = components.First(x => IUsableWrapper.ImplementsIUsableCoolDown(x.GetIl2CppType()));
            interactable = new InteractableCooldown(validInteractable);
        }

        if (validInteractable == null || interactable == null)
            return null;

        GameObjectToInteractable.Add(gameObject, interactable);
        return GameObjectToInteractable[gameObject];
    }
}

/*
public struct IUsableOverrides
{
    private float _usableDistance = 0;
    private ImageNames _useIcon = ImageNames.UseButton;
    private float _percentCool = 0;

    public float UsableDistance
    {
        get => _usableDistance;
        set
        {
            _usableDistance = value;
            isDistanceOverriden = true;
        }
    }

    public ImageNames UseIcon
    {
        get => _useIcon;
        set
        {
            _useIcon = value;
            isIconOverriden = true;
        }
    }

    public float PercentCool
    {
        get => _percentCool;
        set
        {
            _percentCool = value;
            isPercentOverriden = true;
        }
    }

    public bool isDistanceOverriden = false;
    public bool isIconOverriden = false;
    public bool isPercentOverriden = false;

    public IUsableOverrides()
    {

    }
}
*/
