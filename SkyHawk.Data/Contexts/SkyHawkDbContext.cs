using SkyHawk.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace SkyHawk.Data.Contexts;

public class SkyHawkDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ServerInstance> Servers { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<Activity> Activities { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=master;User Id=sa;Password=f66c5b93-1987-4547-aa89-d75c30017b0f;TrustServerCertificate=True",
            x => x.UseDateOnlyTimeOnly()
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServerInstance>()
            .Property(e => e.ContainerId)
            .IsFixedLength();

        modelBuilder.Entity<Snapshot>()
            .Property(e => e.ImageId)
            .IsFixedLength();
    }
}
