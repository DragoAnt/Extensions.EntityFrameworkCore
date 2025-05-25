using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;
using CommonExtensions = DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection.EntityFrameworkCoreExtensions;

namespace DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core static migrations
/// </summary>
public static class EntityFrameworkCoreExtensions
{
    private static readonly SqlServerMigrations Configurator = new();

    /// <summary>
    ///     Use static migrations with specified db context
    /// </summary>
    /// <param name="optionsBuilder">Db contextoptions builder</param>
    /// <param name="initMigrations"></param>
    /// <param name="optionsInit">Static migrations options initialization</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseStaticMigrationsSqlServer(this DbContextOptionsBuilder optionsBuilder,
        Action<StaticMigrationBuilder> initMigrations,
        Action<StaticMigrationsOptions>? optionsInit = null)
    {
        CommonExtensions.UseStaticMigrations(Configurator, optionsBuilder,initMigrations, optionsInit);

        return optionsBuilder;
    }
}