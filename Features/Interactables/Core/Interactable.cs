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

    private IUsable _usable;
    public IUsableOverrides usableOverrides;

    public Interactable(IUsable usable)
    {
        _usable = usable;
        usableOverrides = new IUsableOverrides(usable);
    }

    public IUsable Usable
    {
        get => _usable;
        set
        {
            _usable = value;
            usableOverrides = new IUsableOverrides(value);
        }
    }

    public float MaxDistance
    {
        get => usableOverrides.UsableDistance;
        set => usableOverrides.UsableDistance = value;
    }

    public static Interactable Get(GameObject gameObject)
    {
        if (GameObjectToInteractable.ContainsKey(gameObject))
            return GameObjectToInteractable[gameObject];

        GameObject foundObject = null;
        foreach (Component comp in gameObject.GetComponents<Component>())
        {
            if (comp is not IUsable usable)
                continue;
        }
    }
}

public struct IUsableOverrides
{
    private float _usableDistance;
    private ImageNames _useIcon;
    private float _percentCool;

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

    public IUsableOverrides(IUsable usable)
    {
        _usableDistance = usable.UsableDistance;
        _useIcon = usable.UseIcon;
        _percentCool = usable.PercentCool;
    }
}
