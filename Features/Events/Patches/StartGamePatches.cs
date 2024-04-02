using HarmonyLib;
using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.Helpers.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Events.Patches;

[HarmonyPatch(typeof(AmongUsClient))]
public static class StartGamePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.StartGame))]
    public static bool StartGamePrefix()
    {
        if (!EventManager.ExecuteEvent(new LobbyRoundStarting()))
        {
            GameStartManager manager = null;

            try
            {
                manager = UnityEngine.Object.FindObjectOfType<GameStartManager>();
            }
            catch
            {
                //
            }

            if (manager != null)
                manager.gameObject.SaveObjectFromDestruction();

            return false;
        }

        return true;
    }
}