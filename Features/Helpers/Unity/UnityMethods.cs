using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Helpers.Unity;

/// <summary>
/// Contains events that make it possible to attach into a Unity method without the need of creating a <see cref="MonoBehaviour"/> instance.
/// </summary>
public class UnityMethods : MonoBehaviour
{
    /// <summary>
    /// Only used for making sure that <see cref="UnityMethods"/> was already initialized. In other words, do not change this.
    /// </summary>
    public static UnityMethods Instance;

    public static event Action OnUpdate;
    public static event Action OnLateUpdate;
    public static event Action OnFixedUpdate;

    public void Update()
    {
        OnUpdate?.Invoke();
    }

    public void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    public void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }
}
