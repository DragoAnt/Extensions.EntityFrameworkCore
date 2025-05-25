using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

public interface IEF6MigrationManager
{
    string[] MigrationIds { get; }

    public IEF6HistoryRepository GetRepository(ICurrentDbContext context);
}