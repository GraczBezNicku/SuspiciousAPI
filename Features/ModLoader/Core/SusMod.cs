namespace SuspiciousAPI.Features.ModLoader.Core;

/// <summary>
/// Base class all mods have to inherit
/// </summary>
public abstract class SusMod
{
    public abstract string Name { get; set; }
    public abstract string Description { get; set; }
    public abstract string Version { get; set; }
    public abstract string Author { get; set; }
    public abstract string UUID { get; set; }

    public abstract void Load();
    public abstract void Reload();
    public abstract void Unload();
}
