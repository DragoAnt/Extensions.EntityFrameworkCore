using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

public interface IEF6HistoryRepository
{
    /// <summary>
    ///     Checks whether or not the history table exists.
    /// </summary>
    /// <returns> <see langword="true" /> if the table already exists, <see langword="false" /> otherwise. </returns>
    bool Exists();
        
    /// <summary>
    ///     Queries the history table for all migrations that have been applied.
    /// </summary>
    /// <returns> The list of applied migrations, as <see cref="HistoryRow" /> entities. </returns>
    IEnumerable<string> GetAppliedMigrationIds();
}