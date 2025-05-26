using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;

public interface IStaticMigrationHistoryRepository
{
    /// <summary>
    ///     Queries the history table for all migrations that have been applied.
    /// </summary>
    /// <returns> The list of applied migrations, as <see cref="StaticMigrationHistoryRow" /> entities. </returns>
    IReadOnlyList<StaticMigrationHistoryRow> GetAppliedMigrations();

    /// <summary>
    ///     Queries the history table for all migrations that have been applied.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the list of applied migrations, as <see cref="StaticMigrationHistoryRow" /> entities.
    /// </returns>
    Task<IReadOnlyList<StaticMigrationHistoryRow>> GetAppliedMigrationsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Generates a SQL script to insert a row into the history table.
    /// </summary>
    /// <param name="row"> The row to insert, represented as a <see cref="HistoryRow" /> entity. </param>
    /// <returns> The generated SQL. </returns>
    string GetInsertScript(StaticMigrationHistoryRow row);

    /// <summary>
    ///     Generates a SQL script to delete a row from the history table.
    /// </summary>
    /// <param name="row"> The migration row to delete. </param>
    /// <returns> The generated SQL. </returns>
    string GetDeleteScript(StaticMigrationHistoryRow row);


    /// <summary>
    ///     Checks whether or not the history table exists.
    /// </summary>
    /// <returns> <see langword="true" /> if the table already exists, <see langword="false" /> otherwise. </returns>
    bool Exists();

    /// <summary>
    ///     Checks whether or not the history table exists.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     <see langword="true" /> if the table already exists, <see langword="false" /> otherwise.
    /// </returns>
    Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Overridden by a database provider to generate a SQL script that will create the history table
    ///     if and only if it does not already exist.
    /// </summary>
    /// <returns> The SQL script. </returns>
    string GetCreateIfNotExistsScript();
}

/// <summary>
/// Empty repository for non relational database providers
/// </summary>
public sealed class EmptyStaticMigrationHistoryRepository : IStaticMigrationHistoryRepository
{
    /// <inheritdoc />
    public IReadOnlyList<StaticMigrationHistoryRow> GetAppliedMigrations()
    {
        return ArraySegment<StaticMigrationHistoryRow>.Empty;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<StaticMigrationHistoryRow>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(GetAppliedMigrations());
    }

    /// <inheritdoc />
    public string GetInsertScript(StaticMigrationHistoryRow row)
    {
        return string.Empty;
    }

    /// <inheritdoc />
    public string GetDeleteScript(StaticMigrationHistoryRow row)
    {
        return string.Empty;
    }

    /// <inheritdoc />
    public bool Exists()
    {
        return true;
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Exists());
    }

    /// <inheritdoc />
    public string GetCreateIfNotExistsScript()
    {
        return string.Empty;
    }
}