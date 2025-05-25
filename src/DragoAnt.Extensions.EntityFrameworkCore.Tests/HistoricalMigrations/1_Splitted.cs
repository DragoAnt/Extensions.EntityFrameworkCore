using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests.HistoricalMigrations;

public class DbContext1 : Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected DbContext1()
    {
    }

    /// <inheritdoc />
    public DbContext1(DbContextOptions<DbContext1> options)
        : base(options)
    {
    }
}

[HistoricalMigration(typeof(DbContext0))]
[DbContext(typeof(DbContext1))]
[Migration("10_HistoricalMigration10")]
public class HistoricalMigration10 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}

[DbContext(typeof(DbContext1))]
[Migration("11_Migration11")]
public class Migration11 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}

[DbContext(typeof(DbContext1))]
[Migration("12_Migration12")]
public class Migration12 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}