using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.SqlServer.Enums;

public sealed class EnumsStaticMigrationFactorySqlServer : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EnumsStaticMigrationSqlServer(context, schemaName);
    }
}