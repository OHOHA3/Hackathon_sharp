using Hackathon.Employees;
using Hackathon.Hackathon;
using Hackathon.Hr;
using Hackathon.Strategy;

namespace HackathonTest;

public class HackathonTest
{
    private const double Eps = 1e-3;

    [Test]
    public void Simulate_WithSpecifiedEmployees_HarmonicShouldBeEqual() {
        var juniors = new List<Employee> {
            new(0, "Junior1"),
            new(1, "Junior2"),
            new(2, "Junior3")
        };

        var teamLeads = new List<Employee> {
            new(0, "TeamLead1"),
            new(1, "TeamLead2"),
            new(2, "TeamLead3")
        };
        
        var hrManager = new HrManager(new TeamBuildStrategy());
        var hrDirector = new HrDirector();
        var dbContext = TestDbContextFactory.CreateInMemoryDbContext();
        var hackathon = new HackathonEngine(hrManager, hrDirector, dbContext);
        var result = hackathon.Simulate(teamLeads, juniors);
        Assert.That(result, Is.EqualTo(1.565).Within(Eps));
    }
}