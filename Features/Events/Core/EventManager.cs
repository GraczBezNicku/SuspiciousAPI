using SuspiciousAPI.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Core;

/// <summary>
/// Manages all event related actions.
/// </summary>
public static class EventManager
{
    public static Dictionary<Type, List<Delegate>> EventTypeToDelegates = new Dictionary<Type, List<Delegate>>();
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

        if (!EventTypeToDelegates.ContainsKey(eventType))
        {
            BepInExPlugin.Instance.Log.LogError($"Event of type {eventType} is not registered! (Not a key of EventTypeToDelegates)");
            return true;
        }

        bool anyReturnedFalse = false;
        foreach (Delegate del in EventTypeToDelegates[eventType])
        {
            object returned = del.Method.Invoke(EventHandlers[eventType], new object[] { eventArgs });

            if (returned == null)
                continue;

            anyReturnedFalse = (bool)returned;
        }

        return eventArgs.Cancellable ? !anyReturnedFalse : true;
    }

    public static void RegisterEvents(object mod)
    {
        Assembly targetAssembly = Assembly.GetAssembly(mod.GetType());

        foreach (Type type in targetAssembly.GetTypes())
        {
            if (!type.IsClass)
                continue;

            bool found = false;
            object handler = null;

            foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!IsValidEvent(method, out var eventType))
                    continue;
                
                if (!found)
                {
                    found = true;
                    handler = Activator.CreateInstance(type);
                    EventHandlers.Add(type, handler);
                }

                RegisterEvent(eventType, method, handler);
            }
        }
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
            || args.First().ParameterType != typeof(EventBase))
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

    private static void RegisterEvent(Type eventType, MethodInfo method, object handler)
    {
        if (!EventTypeToDelegates.ContainsKey(eventType))
            EventTypeToDelegates.Add(eventType, new List<Delegate>());

        Delegate del = Delegate.CreateDelegate(method.ReturnType, handler, method.Name);

        if (EventTypeToDelegates[eventType].Contains(del))
            return;

        EventTypeToDelegates[eventType].Add(del);
    }
}
