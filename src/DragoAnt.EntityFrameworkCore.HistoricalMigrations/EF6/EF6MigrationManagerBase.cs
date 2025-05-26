using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations.EF6;

public abstract class EF6MigrationManagerBase : IEF6MigrationManager
{
    /// <inheritdoc />
    public abstract string[] MigrationIds { get; }

    /// <param name="context"></param>
    /// <inheritdoc />
    public IEF6HistoryRepository GetRepository(ICurrentDbContext context)
    {
        return DefaultEF6HistoryRepository.Create(context);
    }
}