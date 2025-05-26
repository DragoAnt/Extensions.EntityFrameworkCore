using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;

namespace DragoAnt.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

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