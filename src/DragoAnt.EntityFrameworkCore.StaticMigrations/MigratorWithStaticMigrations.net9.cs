using Microsoft.EntityFrameworkCore.Migrations;
#if NET9_0
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;
#pragma warning disable EF1001

public class MigratorWithStaticMigrations : BaseMigratorWithStaticMigrations
{
    /// <inheritdoc />
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
            databaseProvider, staticMigrationsService, migrationsModelDiffer, designTimeModel, contextOptions, executionStrategy)
    {
    }

    protected override void PopulateMigrations(IEnumerable<string> appliedMigrationEntries, string? targetMigration, out MigratorData parameters)
    {
        var appliedMigrationEntriesArray = appliedMigrationEntries.ToArray();

        base.PopulateMigrations(appliedMigrationEntriesArray, targetMigration, out parameters);

        var migrationsToApply = parameters.AppliedMigrations;
        var context = new MigrateContext(migrationsToApply, DateTime.UtcNow);

        var beforeMigration = new StaticBeforeOperationsMigration(GetBeforeMigrationOperations(context).ToArray());
        var afterMigration = new StaticAfterOperationsMigration(GetAfterMigrationOperations(context).ToArray());

        parameters = new MigratorData([beforeMigration, ..migrationsToApply, afterMigration], parameters.RevertedMigrations, parameters.TargetMigration);
    }

    protected override IReadOnlyList<MigrationCommand> GenerateUpSql(
        Migration migration,
        MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        return migration is StaticBeforeOperationsMigration or StaticAfterOperationsMigration
            ? _migrationsSqlGenerator.Generate(migration.UpOperations, null, options)
            : base.GenerateUpSql(migration, options);
    }

    /// <inheritdoc />
    protected override IReadOnlyList<HistoryRow> MigrateInternal(string? targetMigration)
    {
        var appliedMigrations = _historyRepository.GetAppliedMigrations();

        MigrateBase(targetMigration);

        return appliedMigrations;
    }

    protected override async Task<IReadOnlyList<HistoryRow>> MigrateInternalAsync(
        string? targetMigration,
        CancellationToken cancellationToken)
    {
        var appliedMigrations = await _historyRepository.GetAppliedMigrationsAsync(cancellationToken);

        await MigrateBaseAsync(targetMigration, cancellationToken);

        return appliedMigrations;
    }
}

#endif