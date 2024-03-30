using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI;

public static class BepInExConfig
{
    private static ConfigEntry<bool> _IgnoreDependencyIssues;
    public static bool IgnoreDependencyIssues
    {
        get
        {
            return _IgnoreDependencyIssues.Value;
        }
    }

    private static ConfigEntry<bool> _DebugMode;
    public static bool DebugMode
    {
        get 
        { 
            return _DebugMode.Value;
        }
    }

    public static void Bind(ConfigFile cfg)
    {
        _IgnoreDependencyIssues = cfg.Bind("Functionality", 
            "IgnoreDependencyIssues", 
            false, 
            "Whether or not the SusAPI's mod loader will ignore dependency issues. (SET TO TRUE ONLY IF YOU KNOW WHAT YOU'RE DOING!)");

        _DebugMode = cfg.Bind("Logging",
            "DebugMode",
            false,
            "Whether or not SuspiciousAPI should send debug messages.");
    }
}
