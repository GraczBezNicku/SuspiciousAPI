using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using SuspiciousAPI.Features.Events;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // LOAD MODS
        EventManager.RegisterEvents(this);

        Log.LogMessage($"Suspicious API has been initialized!");

        EventManager.ExecuteEvent(new SampleEvent()); // FIXME: Remove this after testing, as well as the class and Registering a few lines above. 
    }

    public override bool Unload()
    {
        // UNREGISTER EVENTS, UNLOAD MODS

        _harmony.UnpatchSelf();

        Instance = null;
        return base.Unload();
    }
}
