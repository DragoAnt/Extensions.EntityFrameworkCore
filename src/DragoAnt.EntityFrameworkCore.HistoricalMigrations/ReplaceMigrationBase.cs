using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

public abstract class ReplaceMigrationBase : Migration
{
    /// <inheritdoc />
    protected ReplaceMigrationBase(IHistoryRepository historyRepository,
        IReadOnlyCollection<string> removeMigrationRowIds)
    {
        HistoryRepository = historyRepository;
        RemoveMigrationRowIds = removeMigrationRowIds;
    }

    public IReadOnlyCollection<string> RemoveMigrationRowIds { get; }
    public IHistoryRepository HistoryRepository { get; }
}