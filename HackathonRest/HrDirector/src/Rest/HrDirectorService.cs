using HrDirector.Db;

namespace HrDirector.Rest;

public class HrDirectorService(
    Director.HrDirector director,
    HackathonDbContext dbContext)
{
    public double CalculateHarmonic(TeamsInfo teamsInfo)
    {
        return director.CalculateHarmonic(teamsInfo.Teams, teamsInfo.TeamLeadsWishLists, teamsInfo.JuniorsWishLists);
    }

    public void SaveHackathon(TeamsInfo teamsInfo, double harmonic)
    {
        var hackathonData = new HackathonData { Harmonic = harmonic };
        dbContext.Hackathons.Add(hackathonData);
        dbContext.SaveChanges();

        var teamLeadsData = teamsInfo.TeamLeads
            .Select(teamLead => new EmployeeData
            {
                Name = teamLead.Name,
                IsTeamLead = true,
                Hackathon = hackathonData,
            }).ToList();
        var juniorsData = teamsInfo.Juniors
            .Select(junior => new EmployeeData
            {
                Name = junior.Name,
                IsTeamLead = false,
                Hackathon = hackathonData,
            }).ToList();
        dbContext.Employees.AddRange(teamLeadsData);
        dbContext.Employees.AddRange(juniorsData);
        dbContext.SaveChanges();

        var teamsData = teamsInfo.Teams
            .Select(team => new TeamData
            {
                Junior = juniorsData[team.Junior.Id],
                TeamLead = teamLeadsData[team.TeamLead.Id]
            }).ToList();
        dbContext.Teams.AddRange(teamsData);
        dbContext.SaveChanges();

        var wishListsForTeamLeadData = (from wishList in teamsInfo.TeamLeadsWishLists
            from desired in wishList.DesiredEmployees
            select new WishListData
            {
                Employee = teamLeadsData[wishList.EmployeeId],
                DesiredEmployee = juniorsData[desired],
                Grade = wishList.DesiredEmployees.Length - Array.IndexOf(wishList.DesiredEmployees, desired)
            }).ToList();
        var wishListsForJuniorData = (from wishList in teamsInfo.JuniorsWishLists
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

    public double GetMeanHarmonic()
    {
        var harmonicSum = dbContext.Hackathons.Sum(h => h.Harmonic);
        var hackathonCount = dbContext.Hackathons.Count();
        return harmonicSum / hackathonCount;
    }
}