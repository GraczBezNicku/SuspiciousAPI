using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using SuspiciousAPI.Features.ModLoader;
using SuspiciousAPI.Features.Roles.Core;

namespace SuspiciousAPI;

[BepInProcess("Among Us.exe")]
[BepInPlugin("gbn.suspiciousapi", "Suspicious Modding API", "1.0.0.0")]
public class BepInExPlugin : BasePlugin
{
    public static BepInExPlugin Instance { get; private set; }

    public Harmony _harmony;

    public override void Load()
    {
        Instance = this;

        // Config
        BepInExConfig.Bind(Config);

        // Monos
        ModLoader.RegisterAllMonoBehaviours(this);

        // Harmony
        _harmony = new Harmony($"GBN-SUSPICIOUSAPI");
        _harmony.PatchAll();

        // Mod Loader
        SetupDirectories();
        ModLoader.LoadAllMods();

        // Roles & Teams
        Role.RegisterAllRoles(this);
        Team.RegisterAllTeams(this);

        Log.LogMessage($"Suspicious API has been initialized!");
    }

    public override bool Unload()
    {
        // UNREGISTER EVENTS, UNLOAD MODS

        _harmony.UnpatchSelf();

        Instance = null;
        return base.Unload();
    }

    public void SetupDirectories()
    {
        Log.LogDebug($"Game's current directory is {Paths.GameRootPath}");

        if (!Directory.Exists($@"{Paths.GameRootPath}\SusAPI"))
            Directory.CreateDirectory($@"{Paths.GameRootPath}\SusAPI");

        if (!Directory.Exists($@"{Paths.GameRootPath}\SusAPI\Mods"))
            Directory.CreateDirectory($@"{Paths.GameRootPath}\SusAPI\Mods");

        if (!Directory.Exists($@"{Paths.GameRootPath}\SusAPI\Configs"))
            Directory.CreateDirectory($@"{Paths.GameRootPath}\SusAPI\Configs");

        if (!Directory.Exists($@"{Paths.GameRootPath}\SusAPI\Dependencies"))
            Directory.CreateDirectory($@"{Paths.GameRootPath}\SusAPI\Dependencies");
    }
}

