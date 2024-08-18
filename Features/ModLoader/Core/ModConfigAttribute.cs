namespace SuspiciousAPI.Features.ModLoader.Core;

/// <summary>
/// Attribute used to mark the mod's config field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ModConfig : Attribute
{

}
