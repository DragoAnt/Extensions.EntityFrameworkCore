using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using DragoAnt.Extensions.StaticMigrations;
using DragoAnt.Extensions.StaticMigrations.MigrationConditions;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public record StaticMigrationRunItem<T>(StaticMigrationItem<T> Item, StaticMigrationHistoryRow? Row, bool IsChanged) : IStaticMigrationConditionItem
    where T : IStaticMigration
{
    public bool IsNew => Row is null;

    public T Migration => Item.Migration;
        
    /// <inheritdoc />
    public string Name => Item.Name;

    /// <inheritdoc />
    IImmutableSet<string> IStaticMigrationConditionItem.Tags => Item.Tags;
}

public class StaticMigrationsService : IStaticMigrationsService
{
    private readonly IStaticMigrationHistoryRepository _historyRepository;
    internal StaticMigrationItem<IStaticSqlMigration>[] SQLMigrations { get; }
    internal StaticMigrationItem<IStaticSqlMigration>[] InitialSqlMigrations { get; }

    public StaticMigrationsService(IStaticMigrationHistoryRepository historyRepository,
        ICurrentDbContext currentDbContext,
        IStaticMigrationCollection<IStaticSqlMigration, DbContext> sqlMigrations)
    {
        _historyRepository = historyRepository;
        var dbContext = currentDbContext.Context;

        var migrations = sqlMigrations.Select(item => new StaticMigrationItem<IStaticSqlMigration>(item.Name, item.Factory(dbContext),item.Tags, item.Condition)).ToList();

        SQLMigrations = migrations.Where(m => !m.Migration.IsInitialMigration).ToArray();
        InitialSqlMigrations = migrations.Where(m => m.Migration.IsInitialMigration).ToArray();
    }

    private static StaticMigrationConditionOptions GetOptions(IEnumerable<StaticMigrationRunItem<IStaticSqlMigration>> items, IImmutableSet<string> tags)
    {
        var conditionItems = items.Where(i => i.IsChanged).Cast<IStaticMigrationConditionItem>().ToImmutableArray();
        return new StaticMigrationConditionOptions(conditionItems, tags);
    }

    internal IEnumerable<StaticMigrationRunItem<IStaticSqlMigration>> ConvertToRunItems(IEnumerable<StaticMigrationItem<IStaticSqlMigration>> items)
    {
        var historyRows = _historyRepository.GetAppliedMigrations();
        foreach (var migrationItem in items)
        {
            var row = historyRows.FirstOrDefault(r => r.Name == migrationItem.Name);
            yield return new(migrationItem, row, row == null || !row.Hash.SequenceEqual(migrationItem.GetHash()));
        }
    }

    internal IEnumerable<StaticMigrationRunItem<IStaticSqlMigration>> GetRunItems(
        IEnumerable<StaticMigrationItem<IStaticSqlMigration>> items, 
        bool force, 
        IImmutableSet<string> activeTags)
    {
        var runItems = ConvertToRunItems(items).ToList();
        var hasActions = runItems.Any(i => i.Item.IsAction());
        var options = GetOptions(runItems, activeTags);

        if (!force && !options.ChangedMigrations.Any())
        {
            return [];
        }
        return !hasActions ? runItems : runItems.Where(runItem => runItem.Item.CanRun(options));
    }
        
    public virtual void CheckForSuppressTransaction(string migrationName, MigrationOperation operation)
    {
        if (operation is SqlOperation { SuppressTransaction: true })
        {
            throw new StaticMigrationException(
                $"Migration: {migrationName}. SqlOperation.SuppressTransaction == true only allowed in Initial static migrations. Rewrite your migration to use SuppressTransaction in initial static migrations only.");
        }
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetInitialOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force)
    {
        if (!_historyRepository.Exists())
        {
            yield return CreateIfNotExistsHistoryTable();
        }

        var runItems = GetRunItems(InitialSqlMigrations, force, staticMigrationTags);
        foreach (var runItem in runItems)
        {
            if (runItem.Row != null)
            {
                yield return DeleteHistoryRow(runItem.Row);
            }
            foreach (var operation in runItem.Migration.GetApplyOperations())
            {
                yield return operation;
            }
            yield return InsertHistoryRow(runItem.Item, migrationDate);
        }
    }

    private bool HasInitialMigrationsToRun(IImmutableSet<string> staticMigrationTags, bool force)
    {
        return GetRunItems(InitialSqlMigrations, force, staticMigrationTags).Any();
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetRevertOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force)
    {
        if (!_historyRepository.Exists())
        {
            yield return CreateIfNotExistsHistoryTable();
        }

        force |= HasInitialMigrationsToRun(staticMigrationTags, force);
        var runItems = GetRunItems(SQLMigrations, force, staticMigrationTags).ToList();

        for (var i = runItems.Count - 1; i >= 0; i--)
        {
            var runItem = runItems[i];
            if (runItem.Row != null)
            {
                yield return DeleteHistoryRow(runItem.Row);
            }
            foreach (var operation in runItem.Migration.GetRevertOperations())
            {
                CheckForSuppressTransaction($"{runItem.Name}(Static Revert)", operation);
                yield return operation;
            }
        }
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetApplyOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force)
    {
        var migrationItems = SQLMigrations;
            
        if (!_historyRepository.Exists())
        {
            yield return CreateIfNotExistsHistoryTable();
        }
            
        force |= HasInitialMigrationsToRun(staticMigrationTags, force);
        var runItems = GetRunItems(SQLMigrations, force, staticMigrationTags).ToList();
            
        for (var i = 0; i < runItems.Count; i++)
        {
            var runItem = runItems[i];
            if (runItem.Row != null)
            {
                yield return DeleteHistoryRow(runItem.Row);
            }
            foreach (var operation in runItem.Migration.GetApplyOperations())
            {
                CheckForSuppressTransaction($"{runItem.Name}(Static Apply)", operation);
                yield return operation;
            }
            yield return InsertHistoryRow(runItem.Item, migrationDate);
        }
    }

    private SqlOperation CreateIfNotExistsHistoryTable()
    {
        return new SqlOperation { Sql = _historyRepository.GetCreateIfNotExistsScript() };
    }

    private SqlOperation InsertHistoryRow<T>(StaticMigrationItem<T> migration, DateTime migrationDate)
        where T : IStaticMigration
    {
        return new SqlOperation { Sql = _historyRepository.GetInsertScript(new StaticMigrationHistoryRow(migration.Name, migration.GetHash(), migrationDate)) };
    }

    private SqlOperation DeleteHistoryRow(StaticMigrationHistoryRow row)
    {
        return new SqlOperation { Sql = _historyRepository.GetDeleteScript(row) };
    }
}