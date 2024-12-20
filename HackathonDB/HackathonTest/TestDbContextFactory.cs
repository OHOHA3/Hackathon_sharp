using Hackathon.Db;
using Microsoft.EntityFrameworkCore;

namespace HackathonTest;

public class TestDbContextFactory
{
    public static HackathonDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<HackathonDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new HackathonDbContext(options);

        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
}