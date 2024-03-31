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
}
