using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.EntityConventions.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.Extensions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core static migrations
/// </summary>
public static class EntityFrameworkCoreExtensions
{
    private static readonly SqlServerEntityConventionsProvider Configurator = new();

    /// <summary>
    ///     Use entity conventions with specified db context
    /// </summary>
    /// <param name="optionsBuilder">Db contextoptions builder</param>
    /// <param name="init">Init entity conventions</param>
    /// <param name="optionsInit">Entity conventions options initialization</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseEntityConventionsSqlServer(this DbContextOptionsBuilder optionsBuilder,
        Action<IEntityConventionsBuilder>? init = null,
        Action<EntityConventionsOptions>? optionsInit = null)
    {
        CommonExtensions.UseEntityConventions(Configurator, optionsBuilder, init, optionsInit);

        return optionsBuilder;
    }
}