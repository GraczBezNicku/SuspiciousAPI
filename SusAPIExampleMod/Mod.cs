﻿using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.ModLoader.Core;
using SuspiciousAPI.Features.Roles.Core;

using static SuspiciousAPI.Features.Logger;

namespace SusAPIExampleMod;

public class Mod : SusMod
{
    public override string Name { get; set; } = "Suspicious API Example Mod";
    public override string Description { get; set; } = "Mainly used for testing and examples.";
    public override string Version { get; set; } = "1.0.0.0";
    public override string Author { get; set; } = "GBN";
    public override string UUID { get; set; } = "gbn.suspiciousapi.examplemod";

    [ModConfig]
    public Config Conifg;

    public override void Load()
    {
        if (!Conifg.IsEnabled)
            return;

        EventManager.RegisterEvents(this);
        Role.RegisterAllRoles(this);
        Team.RegisterAllTeams(this);

        LogMessage($"ExampleMod has been initialized!");
    }

    public override void Reload()
    {
        LogMessage($"ExampleMod has been reloaded!");
    }

    public override void Unload()
    {
        LogMessage($"ExampleMod has been unloaded!");
    }
}
