using BepInEx;
using SuspiciousAPI.Features.Helpers;
using SuspiciousAPI.Features.ModLoader;
using SuspiciousAPI.Features.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static SuspiciousAPI.Features.ModLoader.ModLoader;

namespace SuspiciousAPI.Features;

public static class Configs
{
    /// <summary>
    /// Loads the config for a provied <see cref="SusMod"/>.
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="config">Config object.</param>
    public static void LoadConfig(this SusMod mod, out object config)
    {
        config = null;

        Type configType = null;
        FieldInfo configField = null;

        foreach (FieldInfo field in mod.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            ModConfig cfgField = field.GetCustomAttribute<ModConfig>();
            if (cfgField == null)
                continue;

            configField = field;
            configType = field.FieldType;
            break;
        }

        if (configType == null || configField == null)
            return;

        string configDirectory = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}";

        if (!Directory.Exists(configDirectory))
            Directory.CreateDirectory(configDirectory);

        string configPath = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}\config.yml";

        object modConfig = Activator.CreateInstance(configType);

        if (!File.Exists(configPath))
        {
            File.WriteAllText(configPath, YAML.Serialize(modConfig));
        }
        else
        {
            modConfig = YAML.Deserialize(File.ReadAllText(configPath), configType);
        }

        configField.SetValue(mod, modConfig);
        config = modConfig;
    }

    /// <summary>
    /// Saves the config for a provided <see cref="SusMod"/>
    /// </summary>
    /// <param name="mod"></param>
    public static void SaveConfig(this SusMod mod)
    {
        if (!ModInstanceToConfig.ContainsKey(mod))
            return;

        string configDirectory = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}";

        if (!Directory.Exists(configDirectory))
            return;

        string configPath = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}\config.yml";

        File.WriteAllText(configPath, YAML.Serialize(ModInstanceToConfig[mod]));
    }
}
