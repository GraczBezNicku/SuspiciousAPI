using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Core;

/// <summary>
/// Manages all event related actions.
/// </summary>
public static class EventManager
{
    public static Dictionary<Type, List<MethodInfo>> EventTypeToMethodInfo = new Dictionary<Type, List<MethodInfo>>();
    public static Dictionary<Type, object> EventHandlers = new Dictionary<Type, object>();

    /// <summary>
    /// Executes an event with provided <see cref="EventBase"/> argument.
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <returns>If the event is cancelled, returns <see langword="false"/>, otherwise <see langword="true"/>.</returns>
    public static bool ExecuteEvent(EventBase eventArgs)
    {
        if (eventArgs == null)
        {
            BepInExPlugin.Instance.Log.LogError($"eventArgs cannot be null!");
            return true;
        }

        Type eventType = eventArgs.GetType();

        if (!EventTypeToMethodInfo.ContainsKey(eventType))
        {
            BepInExPlugin.Instance.Log.LogError($"Event of type {eventType} is not registered! (Not a key of {nameof(EventTypeToMethodInfo)})");
            return true;
        }

        bool anyReturnedFalse = false;
        foreach (MethodInfo method in EventTypeToMethodInfo[eventType])
        {
            try
            {
                object returned = method.Invoke(EventHandlers[method.DeclaringType], new object[] { eventArgs });

                if (returned == null)
                    continue;

                anyReturnedFalse = anyReturnedFalse ? true : (bool)returned;
            }
            catch (Exception ex)
            {
                BepInExPlugin.Instance.Log.LogError($"Failed executing event for method {method.Name} ({eventArgs.GetType().Name})!\n{ex}");
            }
        }

        return eventArgs.Cancellable ? !anyReturnedFalse : true;
    }

    /// <summary>
    /// Subscribes methods with the <see cref="Event"/> attribute in the provided mod.
    /// </summary>
    /// <param name="mod">Mod's instance, derived from a class that inherits <see cref="ModLoader.Core.SusMod"/>.</param>
    public static void RegisterEvents(object mod)
    {
        Assembly targetAssembly = Assembly.GetAssembly(mod.GetType());

        foreach (Type type in targetAssembly.GetTypes())
        {
            if (!type.IsClass)
                continue;

            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!IsValidEvent(method, out var eventType))
                    continue;
                
                if (!EventHandlers.ContainsKey(type))
                    EventHandlers.Add(type, Activator.CreateInstance(type));

                RegisterEvent(eventType, method);
            }
        }
    }

    /// <summary>
    /// Unsubscribes methods with the <see cref="Event"/> attribute in the provided mod.
    /// </summary>
    /// <param name="mod">Mod's instance, derived from a class that inherits <see cref="ModLoader.Core.SusMod"/>.</param>
    /// <exception cref="NotImplementedException"></exception>
    public static void UnregisterEvents(object mod)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  Determines whether or not a <seealso cref="MethodInfo"/> is a valid event method.
    /// </summary>
    /// <param name="method"></param>
    /// <returns><see langword="true"/> if provided <seealso cref="MethodInfo"/> is a valid event, <see langword="false"/> if not.</returns>
    private static bool IsValidEvent(MethodInfo method, out Type eventType, bool printError = true)
    {
        eventType = null;

        if (method.GetCustomAttribute<Event>() == null)
            return false;

        if (method.ReturnType != typeof(void) && method.ReturnType != typeof(bool))
        {
            if (printError)
            {
                BepInExPlugin.Instance.Log.LogError($"Method {method.Name} has to return void or bool to be a valid event method!");
            }
            return false;
        }

        ParameterInfo[] args = method.GetParameters();

        if (args.Length != 1
            || !args.First().ParameterType.IsSubclassOf(typeof(EventBase)))
        {
            if (printError)
            {
                BepInExPlugin.Instance.Log.LogError($"Method {method.Name} has to have a EventBase inherited class as its only argument.");
            }
            return false;
        }

        eventType = args.First().ParameterType;
        return true;
    }

    private static void RegisterEvent(Type eventType, MethodInfo method)
    {
        if (!EventTypeToMethodInfo.ContainsKey(eventType))
            EventTypeToMethodInfo.Add(eventType, new List<MethodInfo>());

        if (EventTypeToMethodInfo[eventType].Contains(method))
            return;

        EventTypeToMethodInfo[eventType].Add(method);
    }
}
