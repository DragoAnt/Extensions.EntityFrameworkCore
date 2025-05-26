using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations.EF6;

public sealed class DefaultEF6HistoryRepository : IEF6HistoryRepository
{
    public static IEF6HistoryRepository Create(ICurrentDbContext context)
    {
        return new DefaultEF6HistoryRepository(context.GetEF6HistoryRepository());
    }

    private readonly IHistoryRepository _historyRepository;

    public DefaultEF6HistoryRepository(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }


    /// <inheritdoc />
    public bool Exists()
    {
        return _historyRepository.Exists();
    }

    /// <inheritdoc />
    public IEnumerable<string> GetAppliedMigrationIds()
    {
        return _historyRepository.GetAppliedMigrations().Select(r => r.MigrationId).ToList();
    }
}