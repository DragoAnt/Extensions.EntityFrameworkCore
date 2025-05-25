using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public class RelationalDbContextOptionsConfigurator : IDbContextOptionsConfigurator
{
    /// <inheritdoc />
    public void Configure(DbContextOptionsBuilder builder)
    {
        builder.ReplaceService<IMigrator, MigratorWithStaticMigrations>();
    }
}