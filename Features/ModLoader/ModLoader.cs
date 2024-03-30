using BepInEx;
using SuspiciousAPI.Features.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.ModLoader;

public static class ModLoader
{
    public static Dictionary<Type, object> ModInstances = new Dictionary<Type, object>();
    public static List<Assembly> Dependencies = new List<Assembly>();

    /// <summary>
    /// Loads all the mods in SusAPI's mod directory.
    /// </summary>
    public static void LoadAllMods()
    {
        string modsPath = $@"{Paths.GameRootPath}\SusAPI\Mods";

        LoadAllDependencies();

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

            bool couldResolveDeps = ResolveDependencies(assembly, out string depList);
            if (!couldResolveDeps && !BepInExConfig.IgnoreDependencyIssues)
            {
                Logger.LogError($"Couldn't resolve following dependencies for {assembly.FullName}:\n{depList}");
                continue;
            }
            else if (!couldResolveDeps)
            {
                Logger.LogError($"Couldn't resolve following dependencies for {assembly.FullName}:\n{depList}But IgnoreDependencyIssues is enabled! Proceed with caution...");
            }

            try
            {
                SusMod mod = (SusMod)Activator.CreateInstance(modClass);

                foreach (FieldInfo field in modClass.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    ModConfig cfgField = field.GetCustomAttribute<ModConfig>();
                    if (cfgField == null)
                        continue;

                    field.SetValue(mod, SusConfig.LoadConfig(mod, field.FieldType));
                    break;
                }

                ModInstances.Add(modClass, mod);
                mod.Load();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed creating mod instance for {assembly.FullName}!\n{ex}");
                continue;
            }
        }
    }

    /// <summary>
    /// Loads all dependencies in SusAPI's dependency folder.
    /// </summary>
    public static void LoadAllDependencies()
    {
        foreach (string file in Directory.EnumerateFiles($@"{Paths.GameRootPath}\SusAPI\Dependencies", "*.dll").Where(x => x.EndsWith(".dll")))
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(file);
                Assembly dep = Assembly.Load(bytes);

                if (Dependencies.Any(x => x.FullName == dep.FullName))
                    continue;

                Logger.LogDebug($"Loading dependency: {dep.FullName}");
                Dependencies.Add(dep);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed loading dependency: {file}");
            }
        }
    }

    /// <summary>
    /// Verifies presence of dependencies required for the provided <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="dependencyList"></param>
    /// <returns><see langword="true"/> if dependencies are present, otherwise <see langword="false"/>.</returns>
    public static bool ResolveDependencies(Assembly assembly, out string dependencyList)
    {
        ResolveEmbededDependencies(assembly, out dependencyList);

        AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
        Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        bool anyFailed = false;
        foreach (AssemblyName referencedAssembly in referencedAssemblies)
        {
            Logger.LogDebug($"{assembly.FullName} has referenced {referencedAssembly.FullName}");

            if (loadedAssemblies.Any(x => x.FullName == referencedAssembly.FullName))
                continue;

            dependencyList += $"- {referencedAssembly.FullName}\n";
            anyFailed = true;
        }

        return !anyFailed;
    }

    /// <summary>
    /// Resolves embedded dependencies for the provided <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="dependencyList"></param>
    /// <returns><see langword="true"/> if all embedded dependencies were resolved, otherwise <see langword="false"/>.</returns>
    public static bool ResolveEmbededDependencies(Assembly assembly, out string dependencyList)
    {
        Logger.LogDebug($"Resolving Embedded Resources for {assembly.FullName}");

        dependencyList = "";
        bool anyFailed = false;

        try
        {
            string[] resources = assembly.GetManifestResourceNames();

            foreach (string resource in resources)
            {
                if (resource.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    MemoryStream stream = new MemoryStream();
                    Stream data = assembly.GetManifestResourceStream(resource);

                    if (data == null)
                    {
                        Logger.LogDebug($"Could not resolve embeded dependency: {resource}");
                        dependencyList += $"- {resource}\n";
                        anyFailed = true;
                        continue;
                    }

                    data.CopyTo(stream);
                    Dependencies.Add(Assembly.Load(stream.ToArray()));
                }
                else if (resource.EndsWith(".dll.compressed", StringComparison.OrdinalIgnoreCase))
                {
                    Stream data = assembly.GetManifestResourceStream(resource);

                    if (data == null)
                    {
                        Logger.LogDebug($"Could not resolve embeded dependency: {resource}");
                        dependencyList += $"- {resource}\n";
                        anyFailed = true;
                        continue;
                    }

                    DeflateStream deflatedStream = new DeflateStream(data, CompressionMode.Decompress);
                    MemoryStream memoryStream = new MemoryStream();

                    deflatedStream.CopyTo(memoryStream);
                    Dependencies.Add(Assembly.Load(memoryStream.ToArray()));
                }
                Logger.LogDebug($"Successfully resolved embeded dependency: {resource}");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed resolving embedded dependencies! {ex}");
            return false;
        }

        return !anyFailed;
    }
}
