using Microsoft.EntityFrameworkCore;

namespace HrDirector.Db;

public class HackathonDbContext(DbContextOptions<HackathonDbContext> options) : DbContext(options)
{
    public DbSet<HackathonData> Hackathons { get; set; }
    public DbSet<EmployeeData> Employees { get; set; }
    public DbSet<TeamData> Teams { get; set; }
    public DbSet<WishListData> WishLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HackathonData>()
            .HasMany<EmployeeData>()
            .WithOne(e => e.Hackathon)
            .HasForeignKey(e => e.HackathonId);

        modelBuilder.Entity<TeamData>()
            .HasOne(t => t.Junior)
            .WithMany()
            .HasForeignKey(t => t.JuniorId);

        modelBuilder.Entity<TeamData>()
            .HasOne(t => t.TeamLead)
            .WithMany()
            .HasForeignKey(t => t.TeamLeadId);

        modelBuilder.Entity<WishListData>()
            .HasOne(w => w.Employee)
            .WithMany()
            .HasForeignKey(w => w.EmployeeId);

        modelBuilder.Entity<WishListData>()
            .HasOne(w => w.DesiredEmployee)
            .WithMany()
            .HasForeignKey(w => w.DesiredEmployeeId);
    }
}