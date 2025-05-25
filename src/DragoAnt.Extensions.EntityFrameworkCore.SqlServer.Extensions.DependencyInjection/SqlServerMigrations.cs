using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.StaticMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

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