using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection;
using CommonExtensions = DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection.EntityFrameworkCoreExtensions;

namespace DragoAnt.EntityFrameworkCore.InMemory.Extensions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core static migrations
/// </summary>
public static class EntityFrameworkCoreExtensions
{
    private static readonly InMemoryMigrations Configurator = new();

    /// <summary>
    ///     Use static migrations with specified db context
    /// </summary>
    /// <param name="optionsBuilder">Db contextoptions builder</param>
    /// <param name="initMigrations"></param>
    /// <param name="optionsInit">Init static migrations options</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseStaticMigrationsInMemoryDatabase(this DbContextOptionsBuilder optionsBuilder,
        Action<StaticMigrationBuilder>? initMigrations,
        Action<StaticMigrationsOptions>? optionsInit = null)
    {
        var memoryOptionsInit = (Action<StaticMigrationsOptions>)
            (options =>
            {
                options.EnableEnumTables = false;
                optionsInit?.Invoke(options);
            });

        CommonExtensions.UseStaticMigrations(Configurator, optionsBuilder, initMigrations, memoryOptionsInit);

        return optionsBuilder;
    }
}