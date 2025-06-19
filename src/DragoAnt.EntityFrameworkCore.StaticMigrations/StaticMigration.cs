using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

[Migration("StaticBeforeOperationsMigration")]
internal sealed class StaticBeforeOperationsMigration(MigrationOperation[] operations) : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Operations.AddRange(operations);
}

[Migration("StaticAfterOperationsMigration")]
internal sealed class StaticAfterOperationsMigration(MigrationOperation[] operations) : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Operations.AddRange(operations);
}