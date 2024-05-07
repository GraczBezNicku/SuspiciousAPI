using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Roles.Core;

/// <summary>
/// This class represents the most basic representation of a <see cref="Team"/>.
/// </summary>
public abstract class Team
{
    public static HashSet<Team> AllTeams { get; private set; } = new HashSet<Team>();

    public Team()
    {
        AllTeams.Add(this);
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
