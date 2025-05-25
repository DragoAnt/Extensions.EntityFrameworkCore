using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.Extensions.EntityFrameworkCore.InMemory.Extensions.DependencyInjection;

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