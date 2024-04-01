using SuspiciousAPI.Features;
using SuspiciousAPI.Features.Events;
using SuspiciousAPI.Features.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        LogMessage($"A lobby countdown is about to start! If I'm the host, it's gonnna get cancelled");

        if (Lobby.AmHost)
            return false;

        return true;
    }
}
