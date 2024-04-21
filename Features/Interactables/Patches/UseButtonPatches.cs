using HarmonyLib;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using SuspiciousAPI.Features.Interactables.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Interactables.Patches;

/// <summary>
/// This class contains all patches regarding the <see cref="UseButton"/> behaviour
/// </summary>
[HarmonyPatch]
public static class UseButtonPatches
{
    public static IEnumerable<UseButtonSettings> allSettings;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UseButton), nameof(UseButton.Awake))]
    public static void AwakePostfix(UseButton __instance)
    {
        Logger.LogMessage($"Is fastSettings null? {__instance.fastUseSettings == null}");

        allSettings = Enumerable.Empty<UseButtonSettings>();
        foreach (UseButtonSettings sett in __instance.fastUseSettings.Values)
        {
            Logger.LogMessage($"Adding settings to allSettings. {sett.ButtonType}");
            allSettings = allSettings.AddItem(sett);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(UseButton), nameof(UseButton.SetFromSettings))]
    public static bool SetFromSettingsPrefix(UseButton __instance, UseButtonSettings __0)
    {
        Logger.LogMessage($"Settings info:\n" +
            $"ImageName: {__0.ButtonType}\n" +
            $"StringName: {__0.Text}\n" +
            $"Sprite Name: {__0.Image.name}\n" +
            $"Font name: {__0.FontMaterial.name}");

        if (__instance.currentTarget == null)
            return true;

        // Scary D:
        Component targetComp = Il2CppObjectPool.Get<Component>(__instance.currentTarget.Pointer);

        Interactable interactable = Interactable.Get(targetComp);
        if (interactable != null)
        {
            interactable.UseIcon = ImageNames.VitalsButton;

            UseButtonSettings targetSett = allSettings.First(x => x.ButtonType == interactable.UseIcon);
            __instance.graphic.sprite = targetSett.Image;
            __instance.buttonLabelText.fontMaterial = targetSett.FontMaterial;
            __instance.buttonLabelText.text = TranslationController.Instance.GetString(targetSett.Text);
            return false;
        }

        return true;
    }
}
