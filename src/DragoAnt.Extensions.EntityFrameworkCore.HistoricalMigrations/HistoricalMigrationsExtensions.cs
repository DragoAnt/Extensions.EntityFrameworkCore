using System.Reflection;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations;

public static class HistoricalMigrationsExtensions
{
    public static bool HasHistoricalMigrationAttribute(this TypeInfo migration)
    {
        return migration.GetCustomAttribute<HistoricalMigrationAttribute>() is not null;
    }

    public static HistoricalMigrationAttribute GetHistoricalMigrationAttribute(this TypeInfo migration)
    {
        return migration.GetCustomAttribute<HistoricalMigrationAttribute>() ?? throw new InvalidOperationException();
    }
        
    public static InitialMigrationAttribute GetInitialMigration(this TypeInfo migration)
    {
        return migration.GetCustomAttribute<InitialMigrationAttribute>() ?? throw new InvalidOperationException();
    }
}