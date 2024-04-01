using InnerNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GameData;

namespace SuspiciousAPI.Features;

/// <summary>
/// Represents the in-game player.
/// </summary>
public class Player
{
    /// <summary>
    /// Holds all existing players as values, with their game objects as a key.
    /// </summary>
    public static Dictionary<GameObject, Player> GameObjectToPlayer = new Dictionary<GameObject, Player>();

    private PlayerControl _playerControl;

    public Player(PlayerControl control)
    {
        _playerControl = control;
    }

    /// <summary>
    /// Gets the LocalPlayer.
    /// </summary>
    public static Player LocalPlayer => Get(PlayerControl.LocalPlayer);

    /// <summary>
    /// Gets the underlying <see cref="PlayerControl"/> object.
    /// </summary>
    public PlayerControl Control
    {
        get => _playerControl;
    }

    /// <summary>
    /// Gets the <see cref="Player"/>'s name. This may be empty if accessed directly before initialization. (Use <see cref="Events.PlayerInitialized"/> event)
    /// </summary>
    public string Name
    {
        get => _playerControl.Data.PlayerName;
        set => GameData.Instance.UpdateName(_playerControl.PlayerId, value);
    }

    /// <summary>
    /// Returns all existing <see cref="Player"/> objects.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Player> GetPlayers()
    {
        List<Player> players = new List<Player>();

        foreach (PlayerControl ctrl in PlayerControl.AllPlayerControls)
        {
            players.Add(Get(ctrl));
        }

        return players;
    }

    /// <summary>
    /// Gets a <see cref="Player"/> instance based on the provided playerId.
    /// </summary>
    /// <param name="control">playerId belonging to wanted <see cref="Player"/>.</param>
    /// <returns>A <see cref="Player"/> object if found, otherwise <see langword="null"/>.</returns>
    public static Player Get(byte playerId) => Get(GameData.Instance.GetPlayerById(playerId)?.Object);

    /// <summary>
    /// Gets a <see cref="Player"/> instance based on the provided <see cref="ClientData"/>.
    /// </summary>
    /// <param name="control"><see cref="ClientData"/> belonging to wanted <see cref="Player"/>.</param>
    /// <returns>A <see cref="Player"/> object if found, otherwise <see langword="null"/>.</returns>
    public static Player Get(ClientData data) => Get(data?.Character);

    /// <summary>
    /// Gets a <see cref="Player"/> instance based on the provided <see cref="PlayerControl"/>.
    /// </summary>
    /// <param name="control"><see cref="PlayerControl"/> belonging to wanted <see cref="Player"/>.</param>
    /// <returns>A <see cref="Player"/> object if found, otherwise <see langword="null"/>.</returns>
    public static Player Get(PlayerControl control) => Get(control?.gameObject);

    /// <summary>
    /// Gets a <see cref="Player"/> instance based on the provided <see cref="GameObject"/>.
    /// </summary>
    /// <param name="gameObject"><see cref="GameObject"/> belonging to wanted <see cref="Player"/>.</param>
    /// <returns>A <see cref="Player"/> object if found, otherwise <see langword="null"/>.</returns>
    public static Player Get(GameObject gameObject)
    {
        if (GameObjectToPlayer.ContainsKey(gameObject))
            return GameObjectToPlayer[gameObject];

        if (gameObject.TryGetComponent<PlayerControl>(out var playerControl))
        {
            GameObjectToPlayer.Add(gameObject, new Player(playerControl));
            return GameObjectToPlayer[gameObject];
        }

        return null;
    }
}
