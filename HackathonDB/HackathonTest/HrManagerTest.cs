using Hackathon.Employees;
using Hackathon.Hackathon;
using Hackathon.Hr;
using Hackathon.Strategy;
using Moq;

namespace HackathonTest;

public class HrManagerTest
{
    private HrManager _hrManager;

    [SetUp]
    public void Setup()
    {
        _hrManager = new HrManager(new TeamBuildStrategy());
    }

    [Test]
    public void BuildOptimalTeams_WithSpecifiedCount_CountShouldBeEqual()
    {
        var juniors = new List<Employee>
        {
            new(11, "Junior1"),
            new(22, "Junior2")
        };
        var teamLeads = new List<Employee>
        {
            new(1, "TeamLead1"),
            new(2, "TeamLead2")
        };
        var juniorWishlists = new List<Wishlist>
        {
            new(11, [1, 2]),
            new(22, [2, 1])
        };
        var teamLeadWishlists = new List<Wishlist>
        {
            new(1, [11, 22]),
            new(2, [22, 11])
        };

        var teams = _hrManager.BuildOptimalTeams(juniors, teamLeads, teamLeadWishlists, juniorWishlists);
        Assert.That(teams, Has.Count.EqualTo(2));
    }

    [Test]
    public void BuildOptimalTeams_ShouldReturnExpectedTeams()
    {
        var juniors = new List<Employee>
        {
            new(1, "Junior1"),
            new(2, "Junior2"),
            new(3, "Junior3")
        };

        var teamLeads = new List<Employee>
        {
            new(1, "TeamLead1"),
            new(2, "TeamLead2"),
            new(3, "TeamLead3")
        };

        var juniorWishlists = new List<Wishlist>
        {
            new(1, [3, 1, 2]),
            new(2, [2, 3, 1]),
            new(3, [3, 1, 2])
        };

        var teamLeadWishlists = new List<Wishlist>
        {
            new(1, [1, 2, 3]),
            new(2, [2, 3, 1]),
            new(3, [2, 1, 3])
        };

        var teams = _hrManager.BuildOptimalTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);
        for (var i = 0; i < teams.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(juniors[i].Id, Is.EqualTo(teams[i].Junior.Id));
                Assert.That(teamLeads[i].Id, Is.EqualTo(teams[i].TeamLead.Id));
            });
        }
    }

    [Test]
    public void BuildOptimalTeams_ShouldInvokeOnce()
    {
        var juniors = new List<Employee>
        {
            new(0, "Junior1"),
            new(1, "Junior2")
        };
        var teamLeads = new List<Employee>
        {
            new(0, "TeamLead1"),
            new(1, "TeamLead2")
        };

        var strategyMock = new Mock<ITeamBuildingStrategy>();
        strategyMock.Setup(mock => mock.BuildTeams(
                It.IsAny<List<Employee>>(),
                It.IsAny<List<Employee>>(),
                It.IsAny<List<Wishlist>>(),
                It.IsAny<List<Wishlist>>()
            ))
            .Returns([]);

        var manager = new HrManager(strategyMock.Object);
        var director = new HrDirector();
        var dbContext = TestDbContextFactory.CreateInMemoryDbContext();
        var hackathon = new HackathonEngine(manager, director, dbContext);
        hackathon.Simulate(teamLeads, juniors);

        strategyMock.Verify(mock => mock.BuildTeams(
            It.IsAny<List<Employee>>(),
            It.IsAny<List<Employee>>(),
            It.IsAny<List<Wishlist>>(),
            It.IsAny<List<Wishlist>>()
        ), Times.Once);
    }
}