using BepInEx;
using SuspiciousAPI.Features.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.ModLoader;

public static class ModLoader
{
    public static Dictionary<Type, object> ModInstances = new Dictionary<Type, object>();
    public static List<Assembly> Dependencies = new List<Assembly>();

    public static void LoadAllMods()
    {
        string modsPath = $@"{Paths.GameRootPath}\SusAPI\Mods";

        foreach (string path in Directory.EnumerateFiles(modsPath, "*.dll").Where(x => x.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)))
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(File.ReadAllBytes(path));
            }
            catch (Exception ex)
            {
                Logger.LogError($"There has been an error with reading the provided Assembly! ({path})\n{ex}");
                continue;
            }

            Type modClass = null;
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsClass)
                    continue;

                if (type.IsAbstract || type.IsSealed)
                    continue;

                if (type.IsSubclassOf(typeof(SusMod)))
                {
                    modClass = type;
                    break;
                }
            }

            if (modClass == null)
            {
                Logger.LogError($"Assembly does not contain a valid SusMod class! Skipping...");
                continue;
            }

            if (ModInstances.ContainsKey(modClass))
            {
                Logger.LogError($"The mod list already contains found SusMod class. Are you sure there are no duplicates? (Skipping...)");
                continue;
            }

            if (!ResolveDependencies(assembly) && !BepInExConfig.IgnoreDependencyIssues)
            {
                Logger.LogError($"Couldn't resolve dependencies!");
            }

            SusMod mod = (SusMod)Activator.CreateInstance(modClass);

        }
    }

    public static bool ResolveDependencies(Assembly assembly, out string dependencyList)
    {
        // Check if referenced dependencies are in the appdomain, if not, search in EmbededDependencies, if that fails, search in the dependencies folder.
        dependencyList = "";
        return true;
    }

    public static bool ResolveEmbededDependencies(Assembly assembly)
    {
        return true;
    }
}
