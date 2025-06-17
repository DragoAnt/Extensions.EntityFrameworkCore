using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DragoAnt.EntityFrameworkCore.InMemory.DependencyInjection;

public sealed class InMemoryMigrations :IDbContextOptionsConfigurator, IStaticMigrationsProviderConfigurator
{
    public void RegisterServices(IServiceCollection services, StaticMigrationsOptions options)
    {
        if (options.EnableEnumTables)
        {
            services.TryAddTransient<IEnumsStaticMigrationFactory, EnumsStaticMigrationFactoryInMemory>();
        }
    }

    /// <inheritdoc />
    public void Configure(DbContextOptionsBuilder builder)
    {
    }
}