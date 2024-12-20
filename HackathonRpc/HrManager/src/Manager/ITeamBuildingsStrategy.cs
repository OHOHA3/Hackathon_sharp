using Contracts;

namespace HrManager.Manager;

public interface ITeamBuildingStrategy
{
    List<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors,
        List<WishList> teamLeadsWishLists, List<WishList> juniorsWishLists);
}