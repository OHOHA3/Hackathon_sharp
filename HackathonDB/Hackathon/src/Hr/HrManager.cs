using Hackathon.Employees;
using Hackathon.Strategy;

namespace Hackathon.Hr
{
    public class HrManager(ITeamBuildingStrategy strategy)
    {
        public List<Team> BuildOptimalTeams(
            List<Employee> teamLeads, 
            List<Employee> juniors, 
            List<Wishlist> teamLeadsWishlists, 
            List<Wishlist> juniorsWishlists)
        {
            return strategy.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        }
    }
}