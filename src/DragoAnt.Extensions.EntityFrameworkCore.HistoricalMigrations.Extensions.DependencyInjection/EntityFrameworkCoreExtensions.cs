using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.Extensions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core static migrations
/// </summary>
public static class EntityFrameworkCoreExtensions
{
    /// <summary>
    /// Add historical migrations
    /// </summary>
    /// <param name="optionsBuilder">Db context options builder</param>
    /// <param name="init">Historical migrations' options initialization</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseHistoricalMigrations(this DbContextOptionsBuilder optionsBuilder,
        Action<HistoricalMigrationsOptions>? init = null)
    {
        var extension = optionsBuilder.Options.FindExtension<HistoricalMigrationsOptionsExtension>();
        if (extension != null)
        {
            throw new InvalidOperationException("Historical migrations are already registered");
        }

        var options = new HistoricalMigrationsOptions();
        init?.Invoke(options);
        extension = new HistoricalMigrationsOptionsExtension(options);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        optionsBuilder.ReplaceService<IMigrationsAssembly, HistoricalMigrationsAssembly>();
        return optionsBuilder;
    }

    /// <summary>
    /// Add historical migrations and specify DbContext type
    /// </summary>
    /// <param name="optionsBuilder">Db context options builder</param>
    /// <param name="init">Historical migrations' options initialization</param>
    /// <typeparam name="TDbContextHistory">Historical <see cref="DbContext"/> from history migrations assembly</typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static DbContextOptionsBuilder UseHistoricalMigrations<TDbContextHistory>(this DbContextOptionsBuilder optionsBuilder,
        Action<HistoricalMigrationsOptions>? init = null)
        where TDbContextHistory : DbContext
    {
        return optionsBuilder.UseHistoricalMigrations(options =>
        {
            options.DbContextType = typeof(TDbContextHistory);
            init?.Invoke(options);
        });
    }
}