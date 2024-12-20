using System.Text;
using System.Text.Json;
using Contracts;
using Microsoft.Extensions.Logging;

namespace HrManager.Service;

public class HrManagerService(
    ILogger<HrManagerService> logger,
    Manager.HrManager manager,
    HttpClient httpClient)
{
    private static readonly List<WishList> TeamLeadsWishListsQueue = [];
    private static readonly List<WishList> JuniorsWishListsQueue = [];
    private static readonly List<Employee> TeamLeadsQueue = [];
    private static readonly List<Employee> JuniorsQueue = [];

    public void HandleWishLists(EmployeeInfo employeeInfo)
    {
        var employeeWishList = employeeInfo.IsTeamLead ? TeamLeadsWishListsQueue : JuniorsWishListsQueue;
        employeeWishList.Add(employeeInfo.WishList);
        var employeeList = employeeInfo.IsTeamLead ? TeamLeadsQueue : JuniorsQueue;
        employeeList.Add(employeeInfo.Employee);

        if (TeamLeadsWishListsQueue.Count != 5 || JuniorsWishListsQueue.Count != 5) return;

        var teamsInfo = CreateTeams();

        SendTeams(teamsInfo);

        logger.LogInformation("Team information sent");
    }

    private TeamsInfo CreateTeams()
    {
        var teamLeadsWishLists = TeamLeadsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var juniorsWishLists = JuniorsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var teamLeads = TeamLeadsQueue.OrderBy(e => e.Id).ToList();
        var juniors = JuniorsQueue.OrderBy(e => e.Id).ToList();
        var teams = manager.BuildOptimalTeams(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists);
        return new TeamsInfo(teams);
    }

    private static void ClearData()
    {
        TeamLeadsQueue.Clear();
        JuniorsQueue.Clear();
        TeamLeadsWishListsQueue.Clear();
        JuniorsWishListsQueue.Clear();
    }

    private void SendTeams(TeamsInfo teamsInfo)
    {
        const string url = "http://hrdirector:1337/hr_director/teams";
        var json = JsonSerializer.Serialize(teamsInfo);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        ClearData();
        var response = httpClient.PostAsync(url, content).Result;
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Teams sent successfully.");
        }
        else
        {
            logger.LogError($"Failed to send teams. Status code: {response.StatusCode}");
        }
    }
}