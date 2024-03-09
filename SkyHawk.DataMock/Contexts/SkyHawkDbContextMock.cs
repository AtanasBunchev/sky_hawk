using SkyHawk.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace SkyHawk.DataMock.Contexts;

public class SkyHawkDbContextMock : SkyHawkDbContext
{
    private SqliteConnection? dbConn;

    public SkyHawkDbContextMock()
    {
        this.Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Pass an open connection to preserve the in-memory database for the context's lifetime
        // See https://github.com/dotnet/efcore/issues/9842
        this.dbConn = new SqliteConnection("Data Source=:memory:");
        this.dbConn.Open();
        optionsBuilder.UseSqlite(this.dbConn);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");

        builder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter>()
            .HaveColumnType("date");

        base.ConfigureConventions(builder);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (this.dbConn != null)
            this.dbConn.Dispose();
    }
}
