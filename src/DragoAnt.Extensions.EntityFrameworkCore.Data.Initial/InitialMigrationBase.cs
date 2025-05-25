using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;

public abstract class InitialMigrationBase : Migration
{
    private readonly IHistoryRepository _historyRepository;
    private readonly string[] _migrationIds;

    /// <inheritdoc />
    protected InitialMigrationBase(IHistoryRepository historyRepository, string[] migrationIds)
    {
        _historyRepository = historyRepository;
        _migrationIds = migrationIds;
    }
}

[Migration("test")]
class InitialMigrationBaseImpl : InitialMigrationBase
{
    /// <inheritdoc />
    public InitialMigrationBaseImpl(IHistoryRepository historyRepository, string[] migrationIds) 
        : base(historyRepository, migrationIds)
    {
    }

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }
}