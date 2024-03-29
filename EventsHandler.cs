using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.Events;
using SuspiciousAPI.Features;

namespace SuspiciousAPI;

public class EventsHandler
{
    [Event]
    public void ExampleEventWithNoReturnValue(SampleEvent ev)
    {
        Logger.LogInfo($"I'm an event with a void return type!");
    }

    [Event]
    public bool ExampleEventWithBoolReturnValue(SampleEvent ev)
    {
        Logger.LogInfo($"I'm an event with a bool return type! (And I'll cause an exception)");

        EventBase e = null;
        if (e.Cancellable)
            return false;

        return true;
    }
}
