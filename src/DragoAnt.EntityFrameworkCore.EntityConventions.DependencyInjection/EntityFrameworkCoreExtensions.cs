using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for register Entity Framework core entity conventions
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    ///     Use static migrations with specified db context
    /// </summary>
    /// <param name="configurator">Specific provider registration</param>
    /// <param name="optionsBuilder">Db contextoptions builder</param>
    /// <param name="init"></param>
    /// <param name="optionsInit">Static migrations options initialization</param>
    /// <returns></returns>
    public static void UseEntityConventions<TProviderRegistrator>(TProviderRegistrator configurator, DbContextOptionsBuilder optionsBuilder,
        Action<IEntityConventionsBuilder>? init,
        Action<EntityConventionsOptions>? optionsInit = null)
        where TProviderRegistrator : IDbContextOptionsConfigurator, IEntityConventionsProviderConfigurator, new()
    {
        var extension = optionsBuilder.Options.FindExtension<EntityConventionsOptionsExtension>();
        if (extension != null)
        {
            throw new InvalidOperationException("Entity conventions are already registered");
        }

        configurator.Configure(optionsBuilder);

        var options = new EntityConventionsOptions();
        optionsInit?.Invoke(options);
        extension = new EntityConventionsOptionsExtension(configurator, options, init);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
    }
}