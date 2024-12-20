using Hackathon.Db;
using Hackathon.Employees;
using Hackathon.Hr;
using Hackathon.Util;

namespace Hackathon.Hackathon;

public class HackathonEngine(HrManager manager, HrDirector director, HackathonDbContext dbContext)
{
    public double Simulate(List<Employee> teamLeads, List<Employee> juniors)
    {
        var teamLeadsWishLists = WishListCreator.CreateWishList(teamLeads, juniors);
        var juniorsWishLists = WishListCreator.CreateWishList(juniors, teamLeads);
        var teams = manager.BuildOptimalTeams(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists);
        var harmonic = director.CalculateHarmonic(teams, teamLeadsWishLists, juniorsWishLists);

        SaveHackathon(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists, teams, harmonic);

        return harmonic;
    }

    private void SaveHackathon(
        List<Employee> teamLeads,
        List<Employee> juniors,
        List<Wishlist> teamLeadsWishLists,
        List<Wishlist> juniorsWishLists,
        List<Team> teams,
        double harmonic)
    {
        var hackathonData = new HackathonData { Harmonic = harmonic };
        dbContext.Hackathons.Add(hackathonData);
        dbContext.SaveChanges();

        var teamLeadsData = teamLeads
            .Select(teamLead => new EmployeeData
            {
                Name = teamLead.Name,
                IsTeamLead = true,
                Hackathon = hackathonData,
            }).ToList();
        var juniorsData = juniors
            .Select(junior => new EmployeeData
            {
                Name = junior.Name,
                IsTeamLead = false,
                Hackathon = hackathonData,
            }).ToList();
        dbContext.Employees.AddRange(teamLeadsData);
        dbContext.Employees.AddRange(juniorsData);
        dbContext.SaveChanges();

        var teamsData = teams
            .Select(team => new TeamData
            {
                Junior = juniorsData[team.Junior.Id],
                TeamLead = teamLeadsData[team.TeamLead.Id]
            }).ToList();
        dbContext.Teams.AddRange(teamsData);
        dbContext.SaveChanges();

        var wishListsForTeamLeadData = (from wishList in teamLeadsWishLists
            from desired in wishList.DesiredEmployees
            select new WishListData
            {
                Employee = teamLeadsData[wishList.EmployeeId],
                DesiredEmployee = juniorsData[desired],
                Grade = wishList.DesiredEmployees.Length - Array.IndexOf(wishList.DesiredEmployees, desired)
            }).ToList();
        var wishListsForJuniorData = (from wishList in juniorsWishLists
            from desired in wishList.DesiredEmployees
            select new WishListData
            {
                Employee = juniorsData[wishList.EmployeeId],
                DesiredEmployee = teamLeadsData[desired],
                Grade = wishList.DesiredEmployees.Length - Array.IndexOf(wishList.DesiredEmployees, desired)
            }).ToList();
        dbContext.WishLists.AddRange(wishListsForTeamLeadData);
        dbContext.WishLists.AddRange(wishListsForJuniorData);
        dbContext.SaveChanges();
    }
}