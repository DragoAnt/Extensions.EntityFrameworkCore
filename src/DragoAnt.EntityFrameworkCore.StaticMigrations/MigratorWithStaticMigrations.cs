using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using DragoAnt.StaticMigrations.MigrationConditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;
#pragma warning disable EF1001
public class MigratorWithStaticMigrations : Migrator
{
    private readonly IRelationalConnection _connection;
    private readonly ICurrentDbContext _currentContext;
    private readonly IDiagnosticsLogger<DbLoggerCategory.Migrations> _logger;
    private readonly IRelationalCommandDiagnosticsLogger _commandLogger;

    private readonly IHistoryRepository _historyRepository;
    private readonly IRelationalDatabaseCreator _databaseCreator;
    private readonly IMigrationCommandExecutor _migrationCommandExecutor;
    private readonly IMigrationsSqlGenerator _migrationsSqlGenerator;
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

    /// <inheritdoc />
#if NET7_0 || NET8_0
public MigratorWithStaticMigrations(IMigrationsAssembly migrationsAssembly, IHistoryRepository historyRepository, IDatabaseCreator databaseCreator,
        IMigrationsSqlGenerator migrationsSqlGenerator, IRawSqlCommandBuilder rawSqlCommandBuilder, IMigrationCommandExecutor migrationCommandExecutor,
        IRelationalConnection connection, ISqlGenerationHelper sqlGenerationHelper, ICurrentDbContext currentContext,
        IModelRuntimeInitializer modelRuntimeInitializer,
        IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
        IRelationalCommandDiagnosticsLogger commandLogger,
        IDatabaseProvider databaseProvider,
        IStaticMigrationsService staticMigrationsService)
        : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator,
            rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, currentContext, modelRuntimeInitializer, logger, commandLogger,
            databaseProvider)
#else
    public MigratorWithStaticMigrations(
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

    private IStaticMigrationsService StaticMigrationsService { get; }

    /// <inheritdoc />
    public override void Migrate(string? targetMigration = null)
    {
        MigrateGuard(targetMigration);
        var modified = DateTime.UtcNow;

        _logger.MigrateUsingConnection(this, _connection);

        if (!_historyRepository.Exists())
        {
            if (!_databaseCreator.Exists())
            {
                _databaseCreator.Create();
            }

            var command = _rawSqlCommandBuilder.Build(
                _historyRepository.GetCreateScript());

            command.ExecuteNonQuery(
                new RelationalCommandParameterObject(_connection, null, null, _currentContext.Context, _commandLogger));
        }

        var appliedMigrations = _historyRepository.GetAppliedMigrations();
        var migrateContext = GetMigrateContext(appliedMigrations, modified);

        _migrationCommandExecutor.ExecuteNonQuery(GetMigrationCommands(migrateContext), _connection);
    }

    /// <inheritdoc />
    public override async Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
    {
        MigrateGuard(targetMigration);
        var migrationDate = DateTime.UtcNow;

        _logger.MigrateUsingConnection(this, _connection);

        if (!await _historyRepository.ExistsAsync(cancellationToken).ConfigureAwait(false))
        {
            if (!await _databaseCreator.ExistsAsync(cancellationToken).ConfigureAwait(false))
            {
                await _databaseCreator.CreateAsync(cancellationToken).ConfigureAwait(false);
            }

            var command = _rawSqlCommandBuilder.Build(_historyRepository.GetCreateScript());

            await command.ExecuteNonQueryAsync(
                    new RelationalCommandParameterObject(_connection, null, null, _currentContext.Context, _commandLogger),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        var appliedMigrations = await _historyRepository.GetAppliedMigrationsAsync(cancellationToken).ConfigureAwait(false);
        var migrateContext = GetMigrateContext(appliedMigrations, migrationDate);

        await _migrationCommandExecutor.ExecuteNonQueryAsync(GetMigrationCommands(migrateContext), _connection, cancellationToken).ConfigureAwait(false);
    }

    private void MigrateGuard(string? targetMigration)
    {
        if (targetMigration != null)
        {
            throw new ArgumentException("Migrate to targetMigration not supported", nameof(targetMigration));
        }
    }

    private IEnumerable<MigrationCommand> GetMigrationCommands(
        MigrateContext context,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        var migrationsTags = context.GetMigrationsTags();
        foreach (var command in GenerateCommands(StaticMigrationsService.GetInitialOperations(context.MigrationDate, migrationsTags, false).ToList()))
        {
            yield return command;
        }

        foreach (var command in GenerateCommands(StaticMigrationsService.GetRevertOperations(context.MigrationDate, migrationsTags, context.HasMigrations)
                     .ToList()))
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

        foreach (var command in GenerateCommands(StaticMigrationsService.GetApplyOperations(context.MigrationDate, migrationsTags, context.HasMigrations)
                     .ToList()))
        {
            yield return command;
        }
    }

    protected virtual MigrateContext GetMigrateContext(IEnumerable<HistoryRow> appliedMigrationEntries, DateTime migrationDate)
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

        return new MigrateContext(migrationsToApply, migrationDate);
    }

    /// <inheritdoc />
    protected override IReadOnlyList<MigrationCommand> GenerateDownSql(
        Migration migration,
        Migration? previousMigration,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        throw new NotSupportedException("Down migration doesn't supported by migrator with static migrations");
    }

    private IEnumerable<MigrationCommand> GenerateCommands(IReadOnlyList<MigrationOperation> operations)
    {
        return _migrationsSqlGenerator.Generate(operations);
    }

    protected class MigrateContext
    {
        public MigrateContext(IReadOnlyList<Migration> migrationsToApply, DateTime migrationDate)
        {
            MigrationsToApply = migrationsToApply;
            MigrationDate = migrationDate;
        }

        public IReadOnlyList<Migration> MigrationsToApply { get; }
        public DateTime MigrationDate { get; }
        public bool HasMigrations => MigrationsToApply.Count > 0;

        public IImmutableSet<string> GetMigrationsTags()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            return MigrationsToApply.OfType<IWithStaticMigrationActionTag>().SelectMany(m => m.Tags).Distinct().ToImmutableSortedSet();
        }
    }
}