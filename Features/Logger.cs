using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.Tools.Logger;

namespace SuspiciousAPI.Features;

public static class Logger
{ 
    public static void LogMessage(object data) => BepInExPlugin.Instance.Log.LogMessage(data);
    public static void LogInfo(object data) => BepInExPlugin.Instance.Log.LogInfo(data);

    public static void LogDebug(object data, bool debug = false)
    {
        if (!debug)
            return;

        BepInExPlugin.Instance.Log.LogDebug(data);
    }

    public static void LogError(object data) => BepInExPlugin.Instance.Log.LogError(data);
    public static void LogFatal(object data) => BepInExPlugin.Instance.Log.LogFatal(data);
}
