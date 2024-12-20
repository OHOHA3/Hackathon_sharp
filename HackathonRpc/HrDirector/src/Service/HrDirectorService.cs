using Contracts;
using HrDirector.Db;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HrDirector.Service;

public class HrDirectorService(
    Director.HrDirector director,
    HackathonDbContext dbContext,
    IBusControl busControl,
    ILogger<HrDirectorService> logger)
{
    private const int HackathonCount = 10;
    private static int _hackathonNumber;

    private static readonly Lock QueueLock = new();

    private static readonly List<WishList> TeamLeadsWishListsQueue = [];
    private static readonly List<WishList> JuniorsWishListsQueue = [];
    private static readonly List<Employee> TeamLeadsQueue = [];
    private static readonly List<Employee> JuniorsQueue = [];
    private static readonly List<Team> Teams = [];

    public void HandleWishLists(EmployeeInfo employeeInfo)
    {
        var employeeWishList = employeeInfo.IsTeamLead ? TeamLeadsWishListsQueue : JuniorsWishListsQueue;
        employeeWishList.Add(employeeInfo.WishList);
        var employeeList = employeeInfo.IsTeamLead ? TeamLeadsQueue : JuniorsQueue;
        employeeList.Add(employeeInfo.Employee);

        lock (QueueLock)
        {
            if (TeamLeadsWishListsQueue.Count != 5 || JuniorsWishListsQueue.Count != 5 || Teams.IsNullOrEmpty()) return;

            StartWork();
        }
    }

    public void HandleTeams(TeamsInfo teamsInfo)
    {
        Teams.AddRange(teamsInfo.Teams);
        
        lock (QueueLock)
        {
            if (TeamLeadsWishListsQueue.Count != 5 || JuniorsWishListsQueue.Count != 5) return;

            StartWork();
        }
    }

    private void StartWork()
    {
        var teamLeadsWishLists = TeamLeadsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var juniorsWishLists = JuniorsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var teamLeads = TeamLeadsQueue.OrderBy(e => e.Id).ToList();
        var juniors = JuniorsQueue.OrderBy(e => e.Id).ToList();

        var harmonic = CalculateHarmonic(Teams, teamLeadsWishLists, juniorsWishLists);
        logger.LogInformation($"Harmonic of teams: {harmonic}");

        SaveHackathon(teamLeads, juniors, Teams, teamLeadsWishLists, juniorsWishLists, harmonic);

        var meanHarmonic = GetMeanHarmonic();
        logger.LogInformation("mean harmonic: {harmonicSum}", meanHarmonic);

        TeamLeadsQueue.Clear();
        JuniorsQueue.Clear();
        TeamLeadsWishListsQueue.Clear();
        JuniorsWishListsQueue.Clear();
        Teams.Clear();

        if (_hackathonNumber < HackathonCount)
        {
            StartHackathon();
        }
    }

    private double CalculateHarmonic(List<Team> teams, List<WishList> teamLeadsWishLists,
        List<WishList> juniorsWishLists)
    {
        return director.CalculateHarmonic(teams, teamLeadsWishLists, juniorsWishLists);
    }

    private void SaveHackathon(List<Employee> teamLeads, List<Employee> juniors, List<Team> teams,
        List<WishList> teamLeadsWishLists, List<WishList> juniorsWishLists, double harmonic)
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

    private double GetMeanHarmonic()
    {
        var harmonicSum = dbContext.Hackathons.Sum(h => h.Harmonic);
        var hackathonCount = dbContext.Hackathons.Count();
        return harmonicSum / hackathonCount;
    }

    public void StartHackathon()
    {
        logger.LogInformation($"Starting hackathon: {_hackathonNumber + 1}");
        _hackathonNumber += 1;
        busControl.Publish(new StartMessage());
    }
}