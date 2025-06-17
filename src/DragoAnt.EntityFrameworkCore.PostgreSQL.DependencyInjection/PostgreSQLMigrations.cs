using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DragoAnt.EntityFrameworkCore.PostgreSQL.DependencyInjection;

public sealed class PostgreSQLMigrations : RelationalDbContextOptionsConfigurator, IStaticMigrationsProviderConfigurator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection services, StaticMigrationsOptions options)
    {
        services.TryAddTransient<IStaticMigrationHistoryRepository, StaticMigrationHistoryRepositoryPostgreSQL>();

        if (options.EnableEnumTables)
        {
            services.TryAddTransient<IEnumsStaticMigrationFactory, EnumsStaticMigrationFactoryPostgreSQL>();
        }
    }
} 