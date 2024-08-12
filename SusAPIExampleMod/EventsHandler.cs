using BepInEx.Unity.IL2CPP.Utils;
using SuspiciousAPI.Features;
using SuspiciousAPI.Features.Events;
using SuspiciousAPI.Features.Events.Core;
using SuspiciousAPI.Features.Interactables.Core;
using System.Collections;
using UnityEngine;
using static SuspiciousAPI.Features.Logger;

namespace SusAPIExampleMod;

public class EventsHandler
{
    [Event]
    public void OnPlayerInitialized(PlayerInitialized ev)
    {
        LogMessage($"A player has just initialized! Name: {ev.Player.Name}");
    }

    [Event]
    public void OnPlayerJoining(PlayerJoining ev)
    {
        LogMessage($"A player is about to join the game! Name: {ev.PlayerData.PlayerName}");
    }

    [Event]
    public void OnPlayerDisconnected(PlayerDisconnected ev)
    {
        LogMessage($"A player has disconnected from the game! Name: {ev.Player.Name}");
    }

    [Event]
    public void OnLocalPlayerDisconnected(LocalPlayerDisconnected ev)
    {
        LogMessage($"The LocalPlayer has disconnected from the game!");
    }

    [Event]
    public void OnLobbyCountdownStarted(LobbyCountdownStarted ev)
    {
        LogMessage($"A lobby countdown has been started!");
    }

    [Event]
    public bool OnLobbyCountdownStarting(LobbyCountdownStarting ev)
    {
        LogMessage($"A lobby countdown is about to start!");
        return true;
    }

    [Event]
    public bool OnLobbyRoundStarting(LobbyRoundStarting ev)
    {
        LogMessage($"The round is about to start!");

        if (!Lobby.AmHost)
            return true;

        //Lobby.StopLobbyCountdown();
        return true;
    }

    [Event]
    public void OnLobbyRoundStarted(LobbyRoundStarted ev)
    {
        LogMessage($"The round has started!");

        IEnumerator GetInteractable()
        {
            yield return new WaitForSeconds(25f);

            foreach (Interactable i in Interactable.GetInteractables())
            {
                i.UseIcon = ImageNames.CamsButton;
            }
        }

        CoroutineHelper.Instance.StartCoroutine(GetInteractable());
    }
}
