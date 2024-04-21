﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features;

/// <summary>
/// Various utilities regarding the game <see cref="Localization"/>. 
/// </summary>
public static class Localization
{
    public static string GetLocalizedText(StringNames name)
    {
        if (!TranslationController.InstanceExists)
            return string.Empty;

        return TranslationController.Instance.GetString(name);
    }
}
