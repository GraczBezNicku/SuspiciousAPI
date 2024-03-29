using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.ModLoader.Core;

/// <summary>
/// Attribute used to mark the mod's config field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ModConfig : Attribute
{

}
