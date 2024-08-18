namespace SuspiciousAPI.Features.Events.Core;

/// <summary>
/// Base class that all events have to inherit. <see cref="Cancellable"/> is strictly for informational purposes and is ignored in implementation.
/// </summary>
public abstract class EventBase
{
    public virtual bool Cancellable { get; } = false;
}
