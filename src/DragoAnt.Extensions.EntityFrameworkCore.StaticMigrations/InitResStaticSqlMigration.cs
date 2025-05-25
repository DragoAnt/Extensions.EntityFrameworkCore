using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DragoAnt.Shared.Resources;
using DragoAnt.Extensions.StaticMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public sealed class InitResStaticSqlMigration : StaticMigration, IStaticSqlMigration
{
    private readonly ResFile _resFile;
    private readonly bool _suppressTransaction;

    public InitResStaticSqlMigration(ResFile resFile, bool suppressTransaction)
    {
        _resFile = resFile ?? throw new ArgumentNullException(nameof(resFile));
        _suppressTransaction = suppressTransaction;
    }

    /// <inheritdoc />
    public bool IsInitialMigration => true;
        
    protected override byte[] GetHashInternal()
    {
        using var stream = _resFile.ReadStream();
        return HashAlgorithm.ComputeHash(stream);
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetRevertOperations()
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public IEnumerable<MigrationOperation> GetApplyOperations()
    {
        var sql = _resFile.Read();
        yield return new SqlOperation { Sql = sql, SuppressTransaction = _suppressTransaction };
    }
}