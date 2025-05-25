using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests.HistoricalMigrations;

public class DbContext3 : Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected DbContext3()
    {
    }

    /// <inheritdoc />
    public DbContext3(DbContextOptions<DbContext3> options)
        : base(options)
    {
    }
}

    
//NOTE: Case: Split initial(31_HistoricalInitialMigration31) created before get earlier migration(30_Migration30) from long develop feature branch     
[DbContext(typeof(DbContext3))]
[Migration("30_Migration30")]
public class Migration30 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}

[HistoricalMigration(typeof(DbContext2), Initial = true)]
[DbContext(typeof(DbContext3))]
[Migration("31_HistoricalInitialMigration31")]
public class HistoricalInitialMigration31 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}

[DbContext(typeof(DbContext3))]
[Migration("32_Migration32")]
public class Migration32 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}