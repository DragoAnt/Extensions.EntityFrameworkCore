using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;

public static class StaticMigrationBuilderExtensions
{
    /// <summary>
    /// Initialize support for conventions on static migrations side
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <exception cref="StaticMigrationException"></exception>
    public static void AddTriggerBasedEntityConventionsMigrationSqlServer(this StaticMigrationBuilder builder, string name = "#Conventions")
    {
        builder.AddStaticSqlFactory(name, context => new ConventionsStaticMigrationSqlServer(context));
    }
}