using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using SuspiciousAPI.Features;
using SuspiciousAPI.Features.Helpers.AmongUs.IUsable;
using SuspiciousAPI.Features.ModLoader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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

        BepInExConfig.Bind(Config);

        ModLoader.RegisterAllMonoBehaviours(this);

        _harmony = new Harmony($"GBN-SUSPICIOUSAPI");
        _harmony.PatchAll();

        SetupDirectories();

        ModLoader.LoadAllMods();

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
