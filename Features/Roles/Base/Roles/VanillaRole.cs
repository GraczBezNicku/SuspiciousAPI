using AmongUs.GameOptions;
using SuspiciousAPI.Features.Roles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Roles.Base.Roles;

internal abstract class VanillaRole : Role
{
    public abstract RoleTypes OriginalRole { get; set; }

    public VanillaRole(Player p) : base(p)
    {
    }
}
