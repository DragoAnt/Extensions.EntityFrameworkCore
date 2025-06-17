using DragoAnt.EntityFrameworkCore.SqlServer;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

public sealed class SqlServerMigrations : RelationalDbContextOptionsConfigurator, IStaticMigrationsProviderConfigurator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection services, StaticMigrationsOptions options)
    {
        services.TryAddTransient<IStaticMigrationHistoryRepository, StaticMigrationHistoryRepositorySqlServer>();

        if (options.EnableEnumTables)
        {
            services.TryAddTransient<IEnumsStaticMigrationFactory, EnumsStaticMigrationFactorySqlServer>();
        }
    }
}