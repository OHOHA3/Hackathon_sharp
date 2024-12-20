using Contracts;

namespace HrManager.Manager
{
    public class HrManager(ITeamBuildingStrategy strategy)
    {
        public List<Team> BuildOptimalTeams(
            List<Employee> teamLeads,
            List<Employee> juniors,
            List<WishList> teamLeadsWishLists,
            List<WishList> juniorsWishLists)
        {
            return strategy.BuildTeams(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists);
        }
    }
}