using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SuspiciousAPI.Features.ModLoader.Core;

/// <summary>
/// Config file for a <see cref="SusMod"/> instance. Create a class that inherits <see cref="SusConfig"/> and make a field with it that has the <see cref="ModConfig"/> attribute.
/// </summary>
public class SusConfig
{
    private SusMod _mod;
    private bool _configLoaded = false;

    private IDeserializer _Deserializer { get; } = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
    private ISerializer _Serializer { get; } = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();

    public virtual bool IsEnabled { get; set; } = true;
    public virtual bool DebugMode { get; set; } = false;

    public SusConfig(SusMod mod)
    {
        _mod = mod;
    }

    public virtual void SaveConfig()
    {
        if (!_configLoaded)
        {
            BepInExPlugin.Instance.Log.LogError($"Can't save a config that hasn't been loaded yet!");
            return;
        }

        string configPath = Paths.GameRootPath + $@"\SusAPI\Configs\{_mod.UUID}\config.yml";
        File.WriteAllText(configPath, _Serializer.Serialize(this));
    }

    public static SusConfig LoadConfig(SusMod mod) 
    {
        string configDirectory = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}";

        if (!Directory.Exists(configDirectory))
            Directory.CreateDirectory(configDirectory);

        string configPath = Paths.GameRootPath + $@"\SusAPI\Configs\{mod.UUID}\config.yml";

        SusConfig cfg = new SusConfig(mod);
        if (!File.Exists(configPath))
        {
            File.WriteAllText(configPath, cfg._Serializer.Serialize(cfg));
        }
        else
        {
            cfg = cfg._Deserializer.Deserialize<SusConfig>(File.ReadAllText(configPath));
        }

        cfg._configLoaded = true;
        return cfg;
    }
}
