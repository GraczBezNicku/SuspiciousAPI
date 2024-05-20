using HarmonyLib;
using SuspiciousAPI.Features.Roles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Roles.Patches;

[HarmonyPatch(typeof(RoleManager))]
public static class RoleManagerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static bool SelectRolesPrefix()
    {
        AssignRoles();
        return false;
    }

    /// <summary>
    /// Assigns all <see cref="Player"/>s a <see cref="Role"/>. Used at the start of a round.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private static void AssignRoles()
    {
        throw new NotImplementedException("Assigning roles is not implemented yet!");

        List<Team> possibleTeams = new List<Team>();

        foreach (Team team in Team.RegisteredTeams)
        {
            for (int i = 0; i < team.MaxAmount; i++)
                possibleTeams.Add(team);
        }

        foreach (Team team in possibleTeams.ToArray())
        {
            if (UnityEngine.Random.Range(0, 100) > team.Chance)
                possibleTeams.Remove(team);
        }

        List<Role> possibleRoles = new List<Role>();

        foreach (Role role in Role.RegisteredRoles)
        {
            for (int i = 0; i < role.MaxAmount; i++)
                possibleRoles.Add(role);
        }

        foreach (Role role in possibleRoles.ToArray())
        {
            if (UnityEngine.Random.Range(0, 100) > role.Chance)
                possibleRoles.Remove(role);
        }

        // At this point all roles have been chosen and will be assigned sorted by chance. 
        List<Role> roleSortedByChance = possibleRoles.OrderBy(x => x.Chance).ToList();
    }
}