using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.Events;

namespace SuspiciousAPI;

public class EventsHandler
{
    [Event]
    public void ExampleEventWithNoReturnValue(SampleEvent ev)
    {
        BepInExPlugin.Instance.Log.LogInfo($"I'm an event with a void return type!");
    }

    [Event]
    public bool ExampleEventWithBoolReturnValue(SampleEvent ev)
    {
        BepInExPlugin.Instance.Log.LogInfo($"I'm an event with a bool return type! (And I'll cause an exception)");

        EventBase e = null;
        if (e.Cancellable)
            return false;

        return true;
    }
}
