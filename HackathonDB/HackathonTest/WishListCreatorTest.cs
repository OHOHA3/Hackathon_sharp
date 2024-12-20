using Hackathon.Employees;
using Hackathon.Util;

namespace HackathonTest;

public class Tests
{
    [Test]
    public void CreateWishList_SizeEqualsJuniorsCount()
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
        
        var wishlistsForJunior = WishListCreator.CreateWishList(juniors, teamLeads);
        Assert.That(juniors, Has.Count.EqualTo(wishlistsForJunior.Count));
    }

    [Test]
    public void CreateWishList_WithSpecifiedJunior_ShouldBeInList()
    {
        var juniors = new List<Employee>
        {
            new(1, "Junior1"),
            new(2, "Junior2"),
            new(3, "Junior3"),
            new(100, "SpecifiedJunior")
        };
        var teamLeads = new List<Employee>
        {
            new(1, "TeamLead1"),
            new(2, "TeamLead2"),
            new(3, "TeamLead3"),
            new(4, "TeamLead4")
        };
        
        var wishlistsForTeamLeads = WishListCreator.CreateWishList(teamLeads, juniors);
        foreach (var wishlist in wishlistsForTeamLeads)
        {
            Assert.That(wishlist.DesiredEmployees, Contains.Item(100));
        }
    }
}