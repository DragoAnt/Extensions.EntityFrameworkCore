using System.Collections.Immutable;
using DragoAnt.StaticMigrations.MigrationConditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public abstract class BaseMigratorWithStaticMigrations : Migrator
{
    // ReSharper disable InconsistentNaming
    protected readonly IRelationalConnection _connection;
    protected readonly ICurrentDbContext _currentContext;
    protected readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> _logger;
    protected readonly IRelationalCommandDiagnosticsLogger _commandLogger;

    protected readonly IHistoryRepository _historyRepository;
    protected readonly IRelationalDatabaseCreator _databaseCreator;
    protected readonly IMigrationCommandExecutor _migrationCommandExecutor;
    protected readonly IMigrationsSqlGenerator _migrationsSqlGenerator;
    protected readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

    // ReSharper restore InconsistentNaming

    /// <inheritdoc />
#if NET7_0 || NET8_0
    public BaseMigratorWithStaticMigrations(
        IMigrationsAssembly migrationsAssembly,
        IHistoryRepository historyRepository,
        IDatabaseCreator databaseCreator,
        IMigrationsSqlGenerator migrationsSqlGenerator,
        IRawSqlCommandBuilder rawSqlCommandBuilder,
        IMigrationCommandExecutor migrationCommandExecutor,
        IRelationalConnection connection,
        ISqlGenerationHelper sqlGenerationHelper,
        ICurrentDbContext currentContext,
        IModelRuntimeInitializer modelRuntimeInitializer,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
        IRelationalCommandDiagnosticsLogger commandLogger,
        IDatabaseProvider databaseProvider,
        IStaticMigrationsService staticMigrationsService)
        : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator,
            rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, currentContext, modelRuntimeInitializer, logger, commandLogger,
            databaseProvider)
#else
    public BaseMigratorWithStaticMigrations(
        IMigrationsAssembly migrationsAssembly,
        IHistoryRepository historyRepository,
        IDatabaseCreator databaseCreator,
        IMigrationsSqlGenerator migrationsSqlGenerator,
        IRawSqlCommandBuilder rawSqlCommandBuilder,
        IMigrationCommandExecutor migrationCommandExecutor,
        IRelationalConnection connection,
        ISqlGenerationHelper sqlGenerationHelper,
        ICurrentDbContext currentContext,
        IModelRuntimeInitializer modelRuntimeInitializer,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
        IRelationalCommandDiagnosticsLogger commandLogger,
        IDatabaseProvider databaseProvider,
        IStaticMigrationsService staticMigrationsService,
        IMigrationsModelDiffer migrationsModelDiffer,
        IDesignTimeModel designTimeModel,
        IDbContextOptions contextOptions,
        IExecutionStrategy executionStrategy)
        : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator,
            rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, currentContext, modelRuntimeInitializer, logger, commandLogger,
            databaseProvider, migrationsModelDiffer, designTimeModel, contextOptions, executionStrategy)
#endif

    {
        StaticMigrationsService = staticMigrationsService;
        _historyRepository = historyRepository;
        _databaseCreator = (IRelationalDatabaseCreator)databaseCreator;
        _migrationsSqlGenerator = migrationsSqlGenerator;
        _rawSqlCommandBuilder = rawSqlCommandBuilder;
        _migrationCommandExecutor = migrationCommandExecutor;
        _connection = connection;
        _currentContext = currentContext;
        _logger = logger;
        _commandLogger = commandLogger;
    }

    protected IStaticMigrationsService StaticMigrationsService { get; }


    /// <inheritdoc />
    public sealed override void Migrate(string? targetMigration = null)
    {
        var appliedMigrations = MigrateInternal(targetMigration);
        MigratePostProcessing(targetMigration, appliedMigrations);
    }

    protected void MigrateBase(string? targetMigration) => base.Migrate(targetMigration);

    protected virtual void MigratePostProcessing(string? targetMigration, IReadOnlyList<HistoryRow> appliedMigrations)
    {
    }

    protected abstract IReadOnlyList<HistoryRow> MigrateInternal(string? targetMigration);

    /// <inheritdoc />
    public sealed override async Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
    {
        MigrateGuard(targetMigration);
        var migrationDate = DateTime.UtcNow;

        var appliedMigrations = await MigrateInternalAsync(targetMigration, cancellationToken);

        await MigratePostProcessingAsync(targetMigration, appliedMigrations, cancellationToken).ConfigureAwait(false);
    }

    protected Task MigrateBaseAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
    {
        return base.MigrateAsync(targetMigration, cancellationToken);
    }

    protected virtual Task MigratePostProcessingAsync(string? targetMigration, IReadOnlyList<HistoryRow> appliedMigrations, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected abstract Task<IReadOnlyList<HistoryRow>> MigrateInternalAsync(string? targetMigration, CancellationToken cancellationToken);


    /// <inheritdoc />
    protected sealed override IReadOnlyList<MigrationCommand> GenerateDownSql(
        Migration migration,
        Migration? previousMigration,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        throw new NotSupportedException("Down migration doesn't supported by migrator with static migrations");
    }

    private static void MigrateGuard(string? targetMigration)
    {
        if (targetMigration != null)
        {
            throw new ArgumentException("Migrate to targetMigration not supported", nameof(targetMigration));
        }
    }

    protected virtual MigrateContext GetMigrateContext(IEnumerable<HistoryRow> appliedMigrationEntries)
    {
#if NET7_0 || NET8_0
        PopulateMigrations(appliedMigrationEntries.Select(t => t.MigrationId),
            string.Empty,
            out var migrationsToApply,
            out _,
            out _);
#else
        PopulateMigrations(appliedMigrationEntries.Select(t => t.MigrationId),
            string.Empty,
            out var migratorData);

        var migrationsToApply = migratorData.AppliedMigrations;
#endif

        return new MigrateContext(migrationsToApply, DateTime.UtcNow);
    }

    protected IEnumerable<MigrationCommand> GetMigrationCommands(
        MigrateContext context,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        foreach (var command in GenerateCommands([..GetBeforeMigrationOperations(context)]))
        {
            yield return command;
        }

        if (context.HasMigrations)
        {
            foreach (var migration in context.MigrationsToApply)
            {
                var operations = migration.UpOperations;
                foreach (var operation in operations)
                {
                    StaticMigrationsService.CheckForSuppressTransaction(migration.GetId(), operation);
                }

                foreach (var command in GenerateUpSql(migration, options))
                {
                    yield return command;
                }
            }
        }

        foreach (var command in GenerateCommands([..GetAfterMigrationOperations(context)]))
        {
            yield return command;
        }
    }


    protected IEnumerable<MigrationOperation> GetBeforeMigrationOperations(MigrateContext context)
    {
        foreach (var operation in StaticMigrationsService.GetInitialOperations(context.MigrationDate, context.MigrationsTags, false))
        {
            yield return operation;
        }

        foreach (var operation in StaticMigrationsService.GetRevertOperations(context.MigrationDate, context.MigrationsTags, context.HasMigrations))
        {
            yield return operation;
        }
    }

    protected IEnumerable<MigrationOperation> GetAfterMigrationOperations(MigrateContext context)
        => StaticMigrationsService.GetApplyOperations(context.MigrationDate, context.MigrationsTags, context.HasMigrations);

    private IEnumerable<MigrationCommand> GenerateCommands(IReadOnlyList<MigrationOperation> operations)
        => _migrationsSqlGenerator.Generate(operations);

    protected class MigrateContext
    {
        public MigrateContext(IReadOnlyList<Migration> migrationsToApply, DateTime migrationDate)
        {
            MigrationsToApply = migrationsToApply;
            MigrationDate = migrationDate;
            MigrationsTags = MigrationsToApply.OfType<IWithStaticMigrationActionTag>().SelectMany(m => m.Tags).Distinct().ToImmutableSortedSet();
        }

        public IReadOnlyList<Migration> MigrationsToApply { get; }
        public DateTime MigrationDate { get; }
        public bool HasMigrations => MigrationsToApply.Count > 0;
        public IImmutableSet<string> MigrationsTags { get; }
    }
}