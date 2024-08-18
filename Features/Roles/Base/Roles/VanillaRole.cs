using AmongUs.GameOptions;
using SuspiciousAPI.Features.Roles.Core;

namespace SuspiciousAPI.Features.Roles.Base.Roles;

internal abstract class VanillaRole : Role
{
    public abstract RoleTypes OriginalRole { get; set; }

    public VanillaRole(Player p) : base(p)
    {
    }
}
