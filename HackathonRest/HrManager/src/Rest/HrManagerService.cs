using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HrManager.Rest;

public class HrManagerService(Manager.HrManager hrManager, HttpClient httpClient, ILogger<HrManagerService> logger)
{
    private static readonly ConcurrentQueue<WishList> TeamLeadsWishListsQueue = [];
    private static readonly ConcurrentQueue<WishList> JuniorsWishListsQueue = [];
    private static readonly ConcurrentQueue<Employee> TeamLeadsQueue = [];
    private static readonly ConcurrentQueue<Employee> JuniorsQueue = [];

    public void Handle(EmployeeInfo employeeInfo)
    {
        var employeeWishList = employeeInfo.IsTeamLead ? TeamLeadsWishListsQueue : JuniorsWishListsQueue;
        employeeWishList.Enqueue(employeeInfo.WishList);
        var employeeList = employeeInfo.IsTeamLead ? TeamLeadsQueue : JuniorsQueue;
        employeeList.Enqueue(employeeInfo.Employee);

        if (TeamLeadsWishListsQueue.Count != 5 || JuniorsWishListsQueue.Count != 5) return;

        var teamsInfo = CreateTeams();
        
        SendTeams(teamsInfo);

        TeamLeadsQueue.Clear();
        JuniorsQueue.Clear();
        TeamLeadsWishListsQueue.Clear();
        JuniorsWishListsQueue.Clear();
    }

    private TeamsInfo CreateTeams()
    {
        var teamLeadsWishLists = TeamLeadsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var juniorsWishLists = JuniorsWishListsQueue.OrderBy(l => l.EmployeeId).ToList();
        var teamLeads = TeamLeadsQueue.OrderBy(e => e.Id).ToList();
        var juniors = JuniorsQueue.OrderBy(e => e.Id).ToList();
        var teams = hrManager.BuildOptimalTeams(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists);
        return new TeamsInfo(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists, teams);
    }

    private void SendTeams(TeamsInfo teamsInfo)
    {
        const string url = "http://hrdirector:1337/hr_director/teams";
        var json = JsonSerializer.Serialize(teamsInfo);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

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