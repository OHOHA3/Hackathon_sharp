using Hackathon.Employees;

namespace Hackathon.Strategy;

public class TeamBuildStrategy : ITeamBuildingStrategy
{
    public List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, List<Wishlist> teamLeadsWishlists,
        List<Wishlist> juniorsWishlists)
    {
        return teamLeads
            .Select((t, i) => new Team(t, juniors[i]))
            .ToList();
    }
}