using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI;

[BepInPlugin("gbn.suspiciousapi", "Suspicious Modding API", "1.0.0.0")]
public class BepInExPlugin : BasePlugin
{
    public Harmony _harmony;
    public static ManualLogSource log;

    public override void Load()
    {
        log = Log;

        _harmony = new Harmony($"GBN-SUSPICIOUSAPI");
        _harmony.PatchAll();

        // INIT

        Log.LogMessage($"Suspicious API has been initialized!");
    }
}
