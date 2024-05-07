using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuspiciousAPI.Features.Roles.Core;

/// <summary>
/// This class represents the most basic representation of a playable <see cref="Role"/>.
/// </summary>
public abstract class Role
{
    /// <summary>
    /// Holds all <see cref="Role"/>s belonging the the <see cref="Player"/>s. Direct fetching isn't recommended.
    /// </summary>
    public static Dictionary<Player, Role> PlayerToRole { get; private set; } = new Dictionary<Player, Role>();

    public Role()
    {

    }

    /// <summary>
    /// Name of the <see cref="Role"/>.
    /// </summary>
    public abstract string Name { get; set; }

    /// <summary>
    /// Description of the <see cref="Role"/> that is displayed when the game starts.
    /// </summary>
    public abstract string GameStartDescription { get; set; }

    /// <summary>
    /// Description of the <see cref="Role"/> that is displayed on the task list.
    /// </summary>
    public abstract string Description { get; set; }

    /// <summary>
    /// Color of the <see cref="Role"/>.
    /// </summary>
    public abstract Color RoleColor { get; set; }

    /// <summary>
    /// The type this <see cref="Role"/> will use as its base. Must be a type derived from <see cref="RoleBehaviour"/>.
    /// </summary>
    public abstract Type BaseType { get; set; }

    /// <summary>
    /// Which role will the <see cref="Player"/> turn into if they die while being this <see cref="Role"/>.
    /// </summary>
    public abstract Role GhostRole { get; set; }

    /// <summary>
    /// Identifier of the desired <see cref="Core.Team"/>
    /// </summary>
    public abstract string TeamIdentifier { get; set; }

    /// <summary>
    /// Team this player belongs to.
    /// </summary>
    public Team Team
    {
        get
        {
            if (Team.AllTeams.Any(x => x.TeamIdentifier == TeamIdentifier))
                return Team.AllTeams.First(x => x.TeamIdentifier == TeamIdentifier);

            return null;
        }
    }

    /// <summary>
    /// Whether or not this <see cref="Role"/>'s tasks are counted towards the Crewmate task pool.
    /// </summary>
    public abstract bool TasksCountTowardProgress { get; set; }

    /// <summary>
    /// Whether or not this <see cref="Role"/> is affected by vision modifications.
    /// </summary>
    public abstract bool AffectedByLightAffectors { get; set; }

    /// <summary>
    /// Fetches the <see cref="Player"/>'s role.
    /// </summary>
    /// <param name="player">Target <see cref="Player"/></param>
    /// <returns><see cref="Role"/> if one is found, otherwise <see langword="null"/></returns>
    public static Role Get(Player player)
    {
        return PlayerToRole.ContainsKey(player) ? PlayerToRole[player] : null;
    }

    /// <summary>
    /// Assigns a <see cref="Role"/> to the specified <see cref="Player"/>. If they already have one, it will be replaced.
    /// </summary>
    /// <param name="player"></param>
    public virtual void AssignRole(Player player)
    {
        if (PlayerToRole.ContainsKey(player))
            PlayerToRole[player].DestroyRole(player);

        // Assign logic here

        PlayerToRole.Add(player, this);
    }

    /// <summary>
    /// Destroys and cleans up the <see cref="Role"/> instance.
    /// </summary>
    public virtual void DestroyRole(Player player)
    {
        // Destroy logic
        PlayerToRole.Remove(player);
    }
}
