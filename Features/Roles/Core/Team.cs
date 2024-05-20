using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Roles.Core;

/// <summary>
/// This class represents the most basic representation of a <see cref="Team"/>.
/// </summary>
public abstract class Team
{
    public static HashSet<Team> RegisteredTeams { get; private set; } = new HashSet<Team>();

    public Team()
    {
        
    }

    /// <summary>
    /// Internal name for the <see cref="Team"/>.
    /// </summary>
    public abstract string TeamIdentifier { get; set; }

    /// <summary>
    /// Text that is displayed when the <see cref="Team"/> wins.
    /// </summary>
    public abstract string EndScreenText { get; set; }

    /// <summary>
    /// Chance of this <see cref="Team"/> appearing in a round.
    /// </summary>
    public abstract float Chance { get; set; }

    /// <summary>
    /// Maximum number of <see cref="Player"/>s that can become this <see cref="Team"/> at the start.
    /// </summary>
    public abstract int MaxAmount { get; set; }

    /// <summary>
    /// Registeres all <see cref="Team"/>s in the provided mod instance.
    /// </summary>
    /// <param name="mod">Mod instance</param>
    public static void RegisterAllTeams(object mod)
    {
        Assembly ass = mod.GetType().Assembly;

        foreach (Type t in ass.GetTypes())
        {
            if (!t.IsSubclassOf(typeof(Team)))
                continue;

            RegisterTeam(t);
        }
    }

    /// <summary>
    /// Unregisteres all <see cref="Team"/>s in the provided mod instance.
    /// </summary>
    /// <param name="mod">Mod instance</param>
    public static void UnregisterAllTeams(object mod)
    {
        Assembly ass = mod.GetType().Assembly;

        foreach (Type t in ass.GetTypes())
        {
            if (!t.IsSubclassOf(typeof(Team)))
                continue;

            UnregisterTeam(t);
        }
    }

    /// <summary>
    /// Registers a <see cref="Team"/> using the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="roleType"><see cref="Type"/> of the desired <see cref="Role"/></param>
    public static void RegisterTeam(Type teamType)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Unregisters a <see cref="Team"/> using the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="roleType"><see cref="Type"/> of the desired <see cref="Role"/></param>
    public static void UnregisterTeam(Type teamType)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Ends the game with the <see cref="Team"/> winning. By default, shows all <see cref="Player"/>s belonging to the <see cref="Team"/>.
    /// </summary>
    public virtual void Win()
    {
        foreach (Player p in Player.GetPlayers())
        {
            if (Role.Get(p).Team != this)
                continue;

            AddPlayerToEndScreen(p);
        }
    }

    /// <summary>
    /// Adds a <see cref="Player"/> to the end screen.
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to be added.</param>
    protected void AddPlayerToEndScreen(Player player)
    {

    }

    /// <summary>
    /// Removes a <see cref="Player"/> to the end screen.
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to be removed.</param>
    protected void RemovePlayerFromEndScreen(Player player)
    {

    }
}
