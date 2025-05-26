using Microsoft.EntityFrameworkCore.Infrastructure;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using DragoAnt.Shared.Resources;
using DragoAnt.StaticMigrations;
using System.Collections.Immutable;

namespace DragoAnt.EntityFrameworkCore.Tests;

public class StaticMigrationServiceTests
{
    private readonly StaticMigrationsService _staticMigrationsService;
    private readonly HistoryRepositoryMock _staticMigrationHistoryRepository;

    private StaticMigrationCollection<IStaticSqlMigration, Microsoft.EntityFrameworkCore.DbContext> _sqlMigrations { get; set; }

    private static ResStaticSqlMigration migration1 = new(ResFile.Relative(@"EmbeddedMigrations\migration1.sql"), null);
    private static ResStaticSqlMigration migration2 = new(ResFile.Relative(@"EmbeddedMigrations\ActionActivate\migration2.sql"), null);
    private static ResStaticSqlMigration migrationAction = new(ResFile.Relative(@"EmbeddedMigrations\migrationAction.sql"), null);

    public StaticMigrationServiceTests()
    {
        _staticMigrationHistoryRepository = new HistoryRepositoryMock();

        _sqlMigrations = new StaticMigrationCollection<IStaticSqlMigration, Microsoft.EntityFrameworkCore.DbContext>();
        _sqlMigrations.Add("migration1", _ => migration1);
        _sqlMigrations.Add("migration2", _ => migration2);
        _sqlMigrations.Add("migrationAction", _ => migrationAction, x => { return x.ChangedMigrations.Any(i => i.Name.Contains("ActionActivate")); });

        _staticMigrationsService = new StaticMigrationsService(_staticMigrationHistoryRepository, new CurrentDbContextMock(), _sqlMigrations);
    }

    [Fact]
    public void ServiceShouldGetConditionsFromMigrationsCollection()
    {
        _staticMigrationsService.SQLMigrations.Length.Should().NotBe(0);
    }

    [Fact]
    public void ServiceShouldReturnApplyOperations()
    {
        var ops = _staticMigrationsService.GetApplyOperations(DateTime.Now, ImmutableHashSet<string>.Empty, false);
        ops.Count().Should().NotBe(0);
    }

    [Fact]
    public void ServiceShouldReturnChangedMigrations()
    {
        // adding migration1 to history mock. GetChangedMigrations should return only one migration - migration2
        _staticMigrationHistoryRepository.Rows.Add(new StaticMigrationHistoryRow("migration1", migration1.GetHash(), DateTime.Now.AddDays(-1)));

        var allRunItems = _staticMigrationsService.ConvertToRunItems(_staticMigrationsService.SQLMigrations).ToList();
            
        allRunItems.Count.Should().Be(3);
            
        allRunItems.Count(i => !i.IsChanged).Should().Be(1);
        allRunItems.Single(i => !i.IsChanged).Name.Should().Be("migration1");

        allRunItems.Count(i => i.IsChanged).Should().Be(2);
            
        var runItems = _staticMigrationsService.GetRunItems(_staticMigrationsService.SQLMigrations, true, ImmutableHashSet<string>.Empty).ToList();

        runItems.Count.Should().Be(2);
            
        runItems.Count(i => !i.IsChanged).Should().Be(1);
        runItems.Single(i => !i.IsChanged).Name.Should().Be("migration1");

        runItems.Count(i => i.IsChanged).Should().Be(1);
        runItems.Single(i => i.IsChanged).Name.Should().Be("migration2");
    }
}

public class CurrentDbContextMock : ICurrentDbContext
{
    public Microsoft.EntityFrameworkCore.DbContext Context { get; set; } = null!;
}

public class HistoryRepositoryMock : IStaticMigrationHistoryRepository
{
    public List<StaticMigrationHistoryRow> Rows { get; set; } = new List<StaticMigrationHistoryRow>();

    public bool Exists()
    {
        return true;
    }

    public Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public IReadOnlyList<StaticMigrationHistoryRow> GetAppliedMigrations()
    {
        return Rows;
    }

    public Task<IReadOnlyList<StaticMigrationHistoryRow>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public string GetCreateIfNotExistsScript()
    {
        return "GetCreateIfNotExistsScript";
    }

    public string GetDeleteScript(StaticMigrationHistoryRow row)
    {
        throw new NotImplementedException();
    }

    public string GetInsertScript(StaticMigrationHistoryRow row)
    {
        return "GetInsertScript";
    }
}