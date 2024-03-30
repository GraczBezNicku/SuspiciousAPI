using SuspiciousAPI.Features.ModLoader.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SusAPIExampleMod;

public class Config
{
    public bool IsEnabled { get; set; } = true;
    public bool DebugMode { get; set; } = false;

    public int SherrifChance { get; set; } = 100;
}