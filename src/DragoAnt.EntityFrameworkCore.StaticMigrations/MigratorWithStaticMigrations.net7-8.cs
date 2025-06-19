#if NET7_0 || NET8_0
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

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
        IStaticMigrationsService staticMigrationsService)
        : base(migrationsAssembly, historyRepository, databaseCreator, migrationsSqlGenerator,
            rawSqlCommandBuilder, migrationCommandExecutor, connection, sqlGenerationHelper, currentContext, modelRuntimeInitializer, logger, commandLogger,
            databaseProvider, staticMigrationsService)
    {
    }

    /// <inheritdoc />
    protected override IReadOnlyList<HistoryRow> MigrateInternal(string? targetMigration = null)
    {
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
                new RelationalCommandParameterObject(
                    _connection,
                    null,
                    null,
                    _currentContext.Context,
                    _commandLogger, CommandSource.Migrations));
        }

        var appliedMigrations = _historyRepository.GetAppliedMigrations();
        var migrateContext = GetMigrateContext(appliedMigrations);

        _migrationCommandExecutor.ExecuteNonQuery(GetMigrationCommands(migrateContext), _connection);

        return appliedMigrations;
    }


    protected override async Task<IReadOnlyList<HistoryRow>> MigrateInternalAsync(
        string? targetMigration,
        CancellationToken cancellationToken)
    {
        _logger.MigrateUsingConnection(this, _connection);

        if (!await _historyRepository.ExistsAsync(cancellationToken).ConfigureAwait(false))
        {
            if (!await _databaseCreator.ExistsAsync(cancellationToken).ConfigureAwait(false))
            {
                await _databaseCreator.CreateAsync(cancellationToken).ConfigureAwait(false);
            }

            var command = _rawSqlCommandBuilder.Build(
                _historyRepository.GetCreateScript());

            await command.ExecuteNonQueryAsync(
                    new RelationalCommandParameterObject(
                        _connection,
                        null,
                        null,
                        _currentContext.Context,
                        _commandLogger, CommandSource.Migrations),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        var appliedMigrations = await _historyRepository.GetAppliedMigrationsAsync(cancellationToken).ConfigureAwait(false);
        var migrateContext = GetMigrateContext(appliedMigrations);

        await _migrationCommandExecutor.ExecuteNonQueryAsync(GetMigrationCommands(migrateContext), _connection, cancellationToken).ConfigureAwait(false);

        return appliedMigrations;
    }
}

#endif