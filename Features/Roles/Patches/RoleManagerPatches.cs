using HarmonyLib;
using SuspiciousAPI.Features.Roles.Core;

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

        /*
         * Method:
         * 1. Generate weights based on chance numbers
         * 2. Choose teams based on their weights, and remove ones that will pass their limit on next roll
         * 3. Roll roles from chosen teams using weights, removing ones that will pass their limit on next roll.
         * 4. Assign roles randomly.
         */

        List<Team> possibleTeams = new List<Team>();

        foreach (Team team in Team.RegisteredTeams)
        {
            for (int i = 0; i < team.MaxAmount; i++)
                possibleTeams.Add(team);
        }

        foreach (Team team in possibleTeams.ToArray())
        {
            if (UnityEngine.Random.Range(0, 100) > team.Chance)
                possibleTeams.RemoveAll(x => x.GetType() == team.GetType());
        }
    }
}