using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Interactables.Core;

public class InteractableCooldown : Interactable
{
    public InteractableCooldown(object usable) : base(usable)
    {
    }

    protected void PreparePatches()
    {

    }
}
