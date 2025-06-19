using DragoAnt.EntityFrameworkCore.StaticMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace DragoAnt.EntityFrameworkCore.PostgreSQL;

public sealed class NpgsqlMigratorWithStaticMigrations : MigratorWithStaticMigrations
{
    /// <inheritdoc />
#if NET7_0 || NET8_0
    public NpgsqlMigratorWithStaticMigrations(
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
            databaseProvider, staticMigrationsService)
#else
    public NpgsqlMigratorWithStaticMigrations(
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
#endif
    {
    }

#if NET7_0 || NET8_0
    protected override void MigratePostProcessing(string? targetMigration, IReadOnlyList<HistoryRow> appliedMigrations)
    {
        PopulateMigrations(
            appliedMigrations.Select(t => t.MigrationId),
            targetMigration,
            out var migrationsToApply,
            out var migrationsToRevert,
            out _);

        if (migrationsToRevert.Count + migrationsToApply.Count == 0)
        {
            return;
        }

        // If a PostgreSQL extension, enum or range was added, we want Npgsql to reload all types at the ADO.NET level.
        var migrations = migrationsToApply.Count > 0 ? migrationsToApply : migrationsToRevert;
        var reloadTypes = migrations
            .SelectMany(m => m.UpOperations)
            .OfType<AlterDatabaseOperation>()
            .Any(o => o.GetPostgresExtensions().Any() || o.GetPostgresEnums().Any() || o.GetPostgresRanges().Any());

        if (reloadTypes && _connection.DbConnection is NpgsqlConnection npgsqlConnection)
        {
            _connection.Open();
            try
            {
                npgsqlConnection.ReloadTypes();
            }
            catch
            {
                _connection.Close();
            }
        }
    }

    protected override async Task MigratePostProcessingAsync(string? targetMigration, IReadOnlyList<HistoryRow> appliedMigrations, CancellationToken cancellationToken)
    {
        PopulateMigrations(
            appliedMigrations.Select(t => t.MigrationId),
            targetMigration,
            out var migrationsToApply,
            out var migrationsToRevert,
            out _);

        if (migrationsToRevert.Count + migrationsToApply.Count == 0)
        {
            return;
        }

        // If a PostgreSQL extension, enum or range was added, we want Npgsql to reload all types at the ADO.NET level.
        var migrations = migrationsToApply.Count > 0 ? migrationsToApply : migrationsToRevert;
        var reloadTypes = migrations
            .SelectMany(m => m.UpOperations)
            .OfType<AlterDatabaseOperation>()
            .Any(o => o.GetPostgresExtensions().Any() || o.GetPostgresEnums().Any() || o.GetPostgresRanges().Any());

        if (reloadTypes && _connection.DbConnection is NpgsqlConnection npgsqlConnection)
        {
            await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await npgsqlConnection.ReloadTypesAsync().ConfigureAwait(false);
            }
            catch
            {
                await _connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }

#else
    protected override void MigratePostProcessing(string? targetMigration, IReadOnlyList<HistoryRow> appliedMigrations)
    {
        PopulateMigrations(
            appliedMigrations.Select(t => t.MigrationId),
            targetMigration,
            out var migratorData);

        if (migratorData.RevertedMigrations.Count + migratorData.AppliedMigrations.Count == 0)
        {
            return;
        }

        // If a PostgreSQL extension, enum or range was added, we want Npgsql to reload all types at the ADO.NET level.
        var migrations = migratorData.AppliedMigrations.Count > 0 ? migratorData.AppliedMigrations : migratorData.RevertedMigrations;
        var reloadTypes = migrations
            .SelectMany(m => m.UpOperations)
            .OfType<AlterDatabaseOperation>()
            .Any(o => o.GetPostgresExtensions().Any() || o.GetPostgresEnums().Any() || o.GetPostgresRanges().Any());

        if (reloadTypes && _connection.DbConnection is NpgsqlConnection npgsqlConnection)
        {
            _connection.Open();
            try
            {
                npgsqlConnection.ReloadTypes();
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    protected override async Task MigratePostProcessingAsync(
        string? targetMigration,
        IReadOnlyList<HistoryRow> appliedMigrations,
        CancellationToken cancellationToken)
    {
        PopulateMigrations(
            appliedMigrations.Select(t => t.MigrationId),
            targetMigration,
            out var migratorData);

        if (migratorData.RevertedMigrations.Count + migratorData.AppliedMigrations.Count == 0)
        {
            return;
        }

        // If a PostgreSQL extension, enum or range was added, we want Npgsql to reload all types at the ADO.NET level.
        var migrations = migratorData.AppliedMigrations.Count > 0 ? migratorData.AppliedMigrations : migratorData.RevertedMigrations;
        var reloadTypes = migrations
            .SelectMany(m => m.UpOperations)
            .OfType<AlterDatabaseOperation>()
            .Any(o => o.GetPostgresExtensions().Any() || o.GetPostgresEnums().Any() || o.GetPostgresRanges().Any());

        if (reloadTypes && _connection.DbConnection is NpgsqlConnection npgsqlConnection)
        {
            await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await npgsqlConnection.ReloadTypesAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                await _connection.CloseAsync().ConfigureAwait(false);
            }
        }
    }
#endif
}