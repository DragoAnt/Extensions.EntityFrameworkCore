using Microsoft.EntityFrameworkCore;
using CommonExtensions = DragoAnt.EntityFrameworkCore.DependencyInjection.EntityFrameworkCoreExtensions;

namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core static migrations
/// </summary>
public static class NpgsqlEntityFrameworkCoreExtensions
{
    private static readonly NpgsqlMigrations Configurator = new();

    /// <summary>
    ///     Use static migrations with specified db context
    /// </summary>
    /// <param name="optionsBuilder">Db contextoptions builder</param>
    /// <param name="initMigrations"></param>
    /// <param name="optionsInit">Static migrations options initialization</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseStaticMigrationsNpgsql(
        this DbContextOptionsBuilder optionsBuilder,
        Action<StaticMigrationBuilder> initMigrations,
        Action<StaticMigrationsOptions>? optionsInit = null)
    {
        CommonExtensions.UseStaticMigrations(Configurator, optionsBuilder, initMigrations, optionsInit);

        return optionsBuilder;
    }
}