using Contracts;

namespace HrManager.Manager;

public class TeamBuildStrategy : ITeamBuildingStrategy
{
    public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, List<WishList> teamLeadsWishLists,
        List<WishList> juniorsWishLists)
    {
        return teamLeads
            .Select((t, i) => new Team(t, juniors[i]))
            .ToList();
    }
}