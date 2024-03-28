using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Core.Events;

/// <summary>
/// Base class that all events have to inherit. <see cref="Cancellable"/> is strictly for informational purposes and is ignored in implementation.
/// </summary>
public abstract class EventBase
{
    public virtual bool Cancellable { get; } = false;
}
