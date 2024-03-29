using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using SuspiciousAPI.Features.Events;
using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Il2CppSystem.Net.Mime.MediaTypeNames;

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

        _harmony = new Harmony($"GBN-SUSPICIOUSAPI");
        _harmony.PatchAll();

        SetupDirectories();
        ModLoader.LoadAllMods();

        Log.LogMessage($"Suspicious API has been initialized!");

        //EventManager.ExecuteEvent(new SampleEvent()); // FIXME: Remove this after testing, as well as the class and Registering a few lines above. 
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
