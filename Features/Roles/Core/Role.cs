using SuspiciousAPI.Features.Roles.Base.Roles;
using System.Reflection;
using UnityEngine;

namespace SuspiciousAPI.Features.Roles.Core;

/// <summary>
/// This class is the most basic representation of a playable <see cref="Role"/>.
/// </summary>
public abstract class Role
{
    /// <summary>
    /// Holds all Registered <see cref="Role"/>s. 
    /// </summary>
    public static HashSet<Role> RegisteredRoles { get; private set; } = new HashSet<Role>();

    /// <summary>
    /// Holds all <see cref="Role"/>s belonging the the <see cref="Player"/>s. Direct fetching isn't recommended.
    /// </summary>
    public static Dictionary<Player, Role> PlayerToRole { get; private set; } = new Dictionary<Player, Role>();

    public Role(Player p)
    {
        if (p == null)
            return;

        // Handle any required setup that wasn't or couldn't be handled in the AssignRole() method.
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
    /// Chance of the <see cref="Role"/> appearing in a round.
    /// </summary>
    public abstract float Chance { get; set; }

    /// <summary>
    /// How many <see cref="Player"/>s can have this role at the start.
    /// </summary>
    public abstract int MaxAmount { get; set; }

    /// <summary>
    /// Color of the <see cref="Role"/>.
    /// </summary>
    public abstract Color RoleColor { get; set; }

    /// <summary>
    /// The type this <see cref="Role"/> will use as its base. Must be a type derived from <see cref="RoleBehaviour"/>.
    /// </summary>
    public abstract Type BaseType { get; set; }

    /// <summary>
    /// Which role will the <see cref="Player"/> turn into if they die while being this <see cref="Role"/>. If the <see cref="Role"/> cannot be killed or is a ghost, you can leave this empty.
    /// </summary>
    public abstract string GhostRole { get; set; }

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
            if (Team.RegisteredTeams.Any(x => x.TeamIdentifier == TeamIdentifier))
                return Team.RegisteredTeams.First(x => x.TeamIdentifier == TeamIdentifier);

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
    /// Registeres all <see cref="Role"/>s in the provided mod instance.
    /// </summary>
    /// <param name="mod">Mod instance</param>
    public static void RegisterAllRoles(object mod)
    {
        Assembly ass = mod.GetType().Assembly;

        foreach (Type t in ass.GetTypes())
        {
            if (!t.IsSubclassOf(typeof(Role)))
                continue;

            RegisterRole(t);
        }
    }

    /// <summary>
    /// Unregisteres all <see cref="Role"/>s in the provided mod instance.
    /// </summary>
    /// <param name="mod">Mod instance</param>
    public static void UnregisterAllRoles(object mod)
    {
        Assembly ass = mod.GetType().Assembly;

        foreach (Type t in ass.GetTypes())
        {
            if (!t.IsSubclassOf(typeof(Role)))
                continue;

            UnregisterRole(t);
        }
    }

    /// <summary>
    /// Registers a <see cref="Role"/> using the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="roleType"><see cref="Type"/> of the desired <see cref="Role"/></param>
    public static void RegisterRole(Type roleType)
    {
        // Remember to check for name duplication!
        throw new NotImplementedException();
    }

    /// <summary>
    /// Unregisters a <see cref="Role"/> using the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="roleType"><see cref="Type"/> of the desired <see cref="Role"/></param>
    public static void UnregisterRole(Type roleType)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Fetches the <see cref="Player"/>'s <see cref="Role"/> instance.
    /// </summary>
    /// <param name="player">Target <see cref="Player"/></param>
    /// <param name="autoAssign">If the <see cref="Role"/> is vanilla, should it automatically be asigned to this <see cref="Player"/> on being found. Recommended <see langword="true"/></param>
    /// <returns><see cref="Role"/> if one is found, otherwise <see langword="null"/></returns>
    public static Role Get(Player player, bool autoAssign = true)
    {
        return PlayerToRole.ContainsKey(player) ? PlayerToRole[player] : ResolveVanillaRole(player.Control.Data.Role);
    }

    /// <summary>
    /// Fetches a <see cref="Role"/> template instance by its <see cref="Type"/> name.
    /// </summary>
    /// <param name="typeName">Name of the <see cref="Role"/>'s <see cref="Type"/></param>
    /// <returns><see cref="Role"/> template if one is found, otherwise null</returns>
    public static Role GetTemplate(string typeName)
    {
        return RegisteredRoles.Any(x => x.GetType().Name == typeName) ? RegisteredRoles.First(x => x.GetType().Name == typeName) : null;
    }

    /// <summary>
    /// Fetches a <see cref="Role"/> instance for the specified <see cref="RoleBehaviour"/>.
    /// </summary>
    /// <param name="role"><see cref="RoleBehaviour"/> of the desired role</param>
    /// <param name="autoAssign">Whether or not to apply the found <see cref="Role"/> to the <see cref="Player"/> owning this <see cref="RoleBehaviour"/></param>
    /// <returns><see cref="Role"/> if one is found, otherwise <see langword="null"/></returns>
    private static Role ResolveVanillaRole(RoleBehaviour role, bool autoAssign = true)
    {
        foreach (Role r in RegisteredRoles)
        {
            if (!r.GetType().IsSubclassOf(typeof(VanillaRole)))
                continue;

            VanillaRole vanillaRole = r as VanillaRole;

            if (vanillaRole.OriginalRole != role.Role)
                continue;

            return vanillaRole;
        }

        return null;
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

        PlayerToRole.Add(player, (Role)Activator.CreateInstance(GetType(), new object[] { player }));
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
