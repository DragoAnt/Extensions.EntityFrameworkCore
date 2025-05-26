using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

public abstract class InitialReplaceMigration : ReplaceMigrationBase
{
    /// <inheritdoc />
    protected InitialReplaceMigration(
        IHistoryRepository historyRepository,
        IReadOnlyCollection<string> removeMigrationRowIds)
        : base(historyRepository, removeMigrationRowIds)
    {
    }

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var migrationId in RemoveMigrationRowIds)
        {
            var sql = HistoryRepository.GetDeleteScript(migrationId);
            migrationBuilder.Sql(sql);
        }
    }
}