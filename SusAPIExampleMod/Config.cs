using SuspiciousAPI.Features.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusAPIExampleMod;

public class Config : SusConfig
{
    public override bool IsEnabled { get => base.IsEnabled; set => base.IsEnabled = value; }
    public override bool DebugMode { get => base.DebugMode; set => base.DebugMode = value; }

    public int SherrifChance { get; set; } = 100;

    public Config(SusMod mod) : base(mod)
    {

    }
}