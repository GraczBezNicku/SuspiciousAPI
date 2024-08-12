namespace SuspiciousAPI.Features.Events.Core;

/// <summary>
/// Marks a method as an event handler. Does not support static methods.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Event : Attribute
{

}
