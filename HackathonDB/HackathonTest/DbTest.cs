using Hackathon.Db;

namespace HackathonTest;

public class DbTest
{
    [Test]
    public void SaveAndRead_MeanHarmonicShouldBeEqual()
    {
        var hackathon1 = new HackathonData
        {
            Harmonic = 1
        };
        var hackathon2 = new HackathonData
        {
            Harmonic = 2
        };
        
        var dbContext = TestDbContextFactory.CreateInMemoryDbContext();
        dbContext.Hackathons.Add(hackathon1);
        dbContext.Hackathons.Add(hackathon2);
        dbContext.SaveChanges();

        var hackathons = dbContext.Hackathons.ToList();
        Assert.That(hackathons, Has.Count.EqualTo(2));

        var meanHarmonic = dbContext.Hackathons.Average(h => h.Harmonic);
        Assert.That(meanHarmonic, Is.EqualTo(1.5));
    }
}