using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public interface IStaticMigrationsService
{
    IEnumerable<MigrationOperation> GetInitialOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force);
    IEnumerable<MigrationOperation> GetRevertOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force);
    IEnumerable<MigrationOperation> GetApplyOperations(DateTime migrationDate, IImmutableSet<string> staticMigrationTags, bool force);
        
    void CheckForSuppressTransaction(string migrationName, MigrationOperation operation);
}