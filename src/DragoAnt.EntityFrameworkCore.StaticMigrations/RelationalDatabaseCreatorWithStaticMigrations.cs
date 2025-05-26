using System.Collections.Immutable;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public class RelationalDatabaseCreatorWithStaticMigrations : RelationalDatabaseCreator
{
    private readonly IRelationalConnection _connection;
    private readonly IRelationalDatabaseCreator _databaseCreator;
    private readonly IMigrationCommandExecutor _migrationCommandExecutor;
    private readonly IMigrationsSqlGenerator _migrationsSqlGenerator;
    private readonly IServiceProvider _provider;
        
    private IStaticMigrationsService? _migrationsService;
    private IStaticMigrationsService StaticMigrationsService => _migrationsService ??= _provider.GetRequiredService<IStaticMigrationsService>();

    /// <inheritdoc />
    public RelationalDatabaseCreatorWithStaticMigrations(IRelationalDatabaseCreator databaseCreator,
        IServiceProvider provider,
        RelationalDatabaseCreatorDependencies dependencies,
        IRelationalConnection connection,
        IMigrationCommandExecutor migrationCommandExecutor,
        IMigrationsSqlGenerator migrationsSqlGenerator)
        : base(dependencies)
    {
        _connection = connection;
        _databaseCreator = databaseCreator;
        _migrationCommandExecutor = migrationCommandExecutor;
        _migrationsSqlGenerator = migrationsSqlGenerator;
        _provider = provider;
    }

    /// <inheritdoc />
    public override bool Exists()
    {
        return _databaseCreator.Exists();
    }

    /// <inheritdoc />
    public override bool HasTables()
    {
        return _databaseCreator.HasTables();
    }

    /// <inheritdoc />
    public override void Create()
    {
        _databaseCreator.Create();
    }

    /// <inheritdoc />
    public override void Delete()
    {
        _databaseCreator.Delete();
    }

    public override Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.ExistsAsync(cancellationToken);
    }

    public override Task<bool> HasTablesAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.HasTablesAsync(cancellationToken);
    }

    public override Task CreateAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.CreateAsync(cancellationToken);
    }

    public override Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.DeleteAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override void CreateTables()
    {
        var migrationDate = DateTime.UtcNow;

        Execute(GetInitialOperationsList(migrationDate));
        _databaseCreator.CreateTables();
        Execute(GetApplyOperationsList(migrationDate));
    }

    /// <inheritdoc />
    public override async Task CreateTablesAsync(CancellationToken cancellationToken = default)
    {
        var migrationDate = DateTime.UtcNow;
        await ExecuteAsync(GetInitialOperationsList(migrationDate), cancellationToken);
        await _databaseCreator.CreateTablesAsync(cancellationToken);
        await ExecuteAsync(GetApplyOperationsList(migrationDate), cancellationToken);
    }

    /// <inheritdoc />
    public override string GenerateCreateScript()
    {
        var migrationDate = DateTime.UtcNow;
        var sbuilder = new StringBuilder();
            
        foreach (var command in GenerateCommands(GetInitialOperationsList(migrationDate)))
        {
            sbuilder.AppendLine(command.CommandText);
            sbuilder.AppendLine(Dependencies.SqlGenerationHelper.BatchTerminator);
        }
            
        sbuilder.AppendLine(_databaseCreator.GenerateCreateScript());
        sbuilder.AppendLine(Dependencies.SqlGenerationHelper.BatchTerminator);

        foreach (var command in GenerateCommands(GetApplyOperationsList(migrationDate)))
        {
            sbuilder.AppendLine(command.CommandText);
            sbuilder.AppendLine(Dependencies.SqlGenerationHelper.BatchTerminator);
        }
        return sbuilder.ToString();
    }

    private IReadOnlyList<MigrationOperation> GetInitialOperationsList(DateTime migrationDate)
    {
        return StaticMigrationsService.GetInitialOperations(migrationDate, ImmutableSortedSet<string>.Empty, true).ToList();
    }
        
    private IReadOnlyList<MigrationOperation> GetApplyOperationsList(DateTime migrationDate)
    {
        return StaticMigrationsService.GetApplyOperations(migrationDate, ImmutableSortedSet<string>.Empty, true).ToList();
    }
        
    private IEnumerable<MigrationCommand> GenerateCommands(IReadOnlyList<MigrationOperation> operationList)
    {
        return _migrationsSqlGenerator.Generate(operationList);
    }

    private void Execute(IReadOnlyList<MigrationOperation> operations)
    {
        var commands = GenerateCommands(operations);
        _migrationCommandExecutor.ExecuteNonQuery(commands, _connection);
    }

    private async Task ExecuteAsync(IReadOnlyList<MigrationOperation> operations, CancellationToken cancellationToken = default)
    {
        var commands = GenerateCommands(operations);
        await _migrationCommandExecutor.ExecuteNonQueryAsync(commands, _connection, cancellationToken).ConfigureAwait(false);
    }
}