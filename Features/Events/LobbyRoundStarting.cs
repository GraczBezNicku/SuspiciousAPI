using SuspiciousAPI.Features.Events.Core;

namespace SuspiciousAPI.Features.Events;

/*
 * IF HOST:
 * InnerNetClient::SendStartGame() (called by AmongUsClient::StartGame())
 * AmongUsClient::StartGame()
 * AmongUsClient::CoStartGame()
 * AmongUsClient::CoStartGameHost()
 * 
 * IF CLIENT:
 * AmongUsClient::CoStartGame()
 * AmongUsClient::CoStartGameClient()
 */
/// <summary>
/// Called on the server right before the round starts. Called on the clients as soon as the game is about to start. Cancellable only on the server.
/// </summary>
public class LobbyRoundStarting : EventBase
{
    public override bool Cancellable => true;

    public LobbyRoundStarting()
    {

    }
}
