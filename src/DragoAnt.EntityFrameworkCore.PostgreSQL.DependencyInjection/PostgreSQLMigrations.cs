using DragoAnt.EntityFrameworkCore.PostgreSQL;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

public sealed class NpgsqlMigrations : RelationalDbContextOptionsConfigurator, IStaticMigrationsProviderConfigurator
{
    public override void Configure(DbContextOptionsBuilder builder)
    {
        builder.ReplaceService<IMigrator, NpgsqlMigratorWithStaticMigrations>();
    }

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