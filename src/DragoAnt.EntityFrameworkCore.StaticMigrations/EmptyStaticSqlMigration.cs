using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public sealed class EmptyStaticSqlMigration : IStaticSqlMigration
{
    /// <inheritdoc />
    public byte[] GetHash()
    {
        return [];
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetRevertOperations()
    {
        yield break;
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetApplyOperations()
    {
        yield break;
    }
}