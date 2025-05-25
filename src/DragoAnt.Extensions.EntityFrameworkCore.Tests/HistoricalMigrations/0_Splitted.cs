using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests.HistoricalMigrations;

public class DbContext0 : Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected DbContext0()
    {
    }

    /// <inheritdoc />
    public DbContext0(DbContextOptions<DbContext0> options)
        : base(options)
    {
    }
}

[DbContext(typeof(DbContext0))]
[Migration("00_StartInitialMigration")]
public class StartInitialMigration00 :Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}
    
[DbContext(typeof(DbContext0))]
[Migration("01_Migration01")]
public class Migration01 :Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}