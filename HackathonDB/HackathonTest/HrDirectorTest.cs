using Hackathon.Employees;
using Hackathon.Hr;

namespace HackathonTest;

public class HrDirectorTest
{
    private HrDirector _hrDirector;
    private const double Eps = 1e-5;

    [SetUp]
    public void Setup()
    {
        _hrDirector = new HrDirector();
    }

    [Test]
    public void CalculateHarmonicMean_ForEqualNumbers_ShouldReturnEqualValue()
    {
        var teams = new List<Team>
        {
            new(new Employee(1, "TeamLead1"), new Employee(1, "Junior1")),
            new(new Employee(2, "TeamLead2"), new Employee(2, "Junior2")),
            new(new Employee(3, "TeamLead3"), new Employee(3, "Junior3"))
        };
        var juniorWishlists = new List<Wishlist>
        {
            new(1, [1, 2, 3]),
            new(2, [2, 3, 1]),
            new(3, [3, 2, 1])
        };
        var teamLeadWishlists = new List<Wishlist>
        {
            new(1, [1, 2, 3]),
            new(2, [2, 3, 1]),
            new(3, [3, 2, 1])
        };

        var harmonicMean = _hrDirector.CalculateHarmonic(teams, juniorWishlists, teamLeadWishlists);
        Assert.That(harmonicMean, Is.EqualTo(3.0).Within(Eps));
    }

    [Test]
    public void CalculateHarmonicMean_WithSpecificValues_ShouldReturnCorrectMean()
    {
        var teams = new List<Team>
        {
            new(new Employee(1, "TeamLead1"), new Employee(1, "Junior1")),
            new(new Employee(2, "TeamLead2"), new Employee(2, "Junior2"))
        };
        var juniorWishlists = new List<Wishlist>
        {
            new(1, [2, 1]),
            new(2, [2, 1])
        };
        var teamLeadWishlists = new List<Wishlist>
        {
            new(1, [1, 2]),
            new(2, [2, 1])
        };

        var harmonicMean = _hrDirector.CalculateHarmonic(teams, juniorWishlists, teamLeadWishlists);
        Assert.That(harmonicMean, Is.EqualTo(1.6).Within(Eps));
    }
}