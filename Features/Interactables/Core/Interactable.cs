using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime;
using SuspiciousAPI.Features.Helpers.AmongUs.IUsable;
using SuspiciousAPI.Features.Interactables.Patches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static SuspiciousAPI.Features.Helpers.IL2CPP.TypeHelpers;

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

        Usable = usable;
    }

    /// <summary>
    /// Prepares all patches and values for the <see cref="Interactable"/> object.
    /// </summary>
    private void Init()
    {
        try
        {
            PrepareValues();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Caught an exception in Interactable::Init()\n{ex}");
        }
    }

    /// <summary>
    /// Prepares the freshly created Interactable by setting its values to the base-game object's values. This method assumes that the provided <see cref="Usable"/> is a valid IUsable.
    /// </summary>
    private void PrepareValues()
    {
        string[] targetProperties =
        {
            "UsableDistance",
            "PercentCool",
            "UseIcon"
        };

        foreach (string propertyName in targetProperties)
        {
            Il2CppSystem.Reflection.PropertyInfo property = UsableScriptType.GetProperty(propertyName, Il2CppSystem.Reflection.BindingFlags.Public | Il2CppSystem.Reflection.BindingFlags.Instance);

            if (property == null)
            {
                Logger.LogError($"A presumed IUsable script lacks a IUsable property! ({propertyName})");
                continue;
            }

            Il2CppSystem.Object value = property.GetValue(Usable as Il2CppSystem.Object);

            switch (propertyName)
            {
                case "UsableDistance": UsableDistance = value.Unbox<float>(); break;
                case "PercentCool": PercentCool = value.Unbox<float>(); break;
                case "UseIcon": UseIcon = value.Unbox<ImageNames>(); break;
            }
        }

        Logger.LogMessage($"Read values:\nDistance: {UsableDistance}\nPercentCool: {PercentCool}\nUseIcon: {UseIcon}");
    }

    /// <summary>
    /// Returns the original script's type. Used in conjunction with <see cref="Usable"/>
    /// </summary>
    public Il2CppSystem.Type UsableScriptType
    {
        get
        {
            return _usableScriptType;
        }
    }
    private Il2CppSystem.Type _usableScriptType;

    /// <summary>
    /// Returns the original script belonging to said Usable. You'll need to cast it to the correct type using the <see cref="UsableScriptType"/> property. 
    /// </summary>
    public object Usable
    {
        get => _usable;
        private set
        {
            _usable = value;
            _usableScriptType = (value as Il2CppSystem.Object).GetIl2CppType();
        }
    }
    private object _usable;

    /// <summary>
    /// Gets or sets the distance an Interactable can be used from.
    /// </summary>
    public float UsableDistance
    {
        get => _usableDistance;
        set => _usableDistance = value;
    }
    private float _usableDistance;

    public float PercentCool
    {
        get => _percentCool;
        set => _percentCool = value;
    }
    private float _percentCool;

    /// <summary>
    /// Gets or sets the use icon an Interactable uses.
    /// </summary>
    public ImageNames UseIcon
    {
        get => _useIcon;
        set => _useIcon = value;
    }
    private ImageNames _useIcon;

    /// <summary>
    /// Gets all the <see cref="Interactable"/> objects present on the map.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Interactable> GetInteractables()
    {
        List<Interactable> interactables = new List<Interactable>();

        List<Component> validUsables = new List<Component>();

        foreach (Type type in IUsableWrapper.KnownIUsableImplementers)
        {
            Il2CppSystem.Object[] usables = UnityEngine.Object.FindObjectsOfType(Il2CppType.From(type));
            foreach (Il2CppSystem.Object obj in usables)
                validUsables.Add(obj as Component);
        }

        foreach (Component comp in validUsables)
            interactables.Add(Get(comp));

        return interactables;
    }

    /// <summary>
    /// Gets the FIRST <see cref="Interactable"/> object present on a <see cref="GameObject"/> that this <see cref="Component"/> belongs to. 
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Interactable Get(Component component) => Get(component.gameObject);

    /// <summary>
    /// Gets the FIRST <see cref="Interactable"/> object present on a <see cref="GameObject"/>.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns><see cref="Interactable"/> if found, <see langword="null"/> if not.</returns>
    public static Interactable Get(GameObject gameObject, bool autoInit = true)
    {
        if (gameObject == null)
            return null;

        if (GameObjectToInteractable.ContainsKey(gameObject))
            return GameObjectToInteractable[gameObject];

        Component[] components = gameObject.GetComponents<Component>();
        Interactable interactable = null;

        if (components.Any(x => IUsableWrapper.KnownIUsableImplementers.Contains(x.GetIl2CppType().GetManagedType())))
        {
            Component implementer = components.First(x => IUsableWrapper.KnownIUsableImplementers.Contains(x.GetIl2CppType().GetManagedType()));
            if (IUsableWrapper.KnownIUsableCooldownImplementers.Contains(implementer.GetIl2CppType().GetManagedType()))
                interactable = new InteractableCooldown(implementer);
            else
                interactable = new Interactable(implementer);
        }

        if (interactable == null)
            return null;

        GameObjectToInteractable.Add(gameObject, interactable);

        if (autoInit)
            interactable.Init();

        return GameObjectToInteractable[gameObject];
    }
}
