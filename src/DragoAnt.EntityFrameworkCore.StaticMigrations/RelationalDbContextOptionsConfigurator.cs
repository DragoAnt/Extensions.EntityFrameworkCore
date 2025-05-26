using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public class RelationalDbContextOptionsConfigurator : IDbContextOptionsConfigurator
{
    /// <inheritdoc />
    public void Configure(DbContextOptionsBuilder builder)
    {
        builder.ReplaceService<IMigrator, MigratorWithStaticMigrations>();
    }
}