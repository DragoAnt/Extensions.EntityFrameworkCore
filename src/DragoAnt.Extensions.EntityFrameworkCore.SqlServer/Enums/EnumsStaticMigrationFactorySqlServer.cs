using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Enums;

public sealed class EnumsStaticMigrationFactorySqlServer : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EnumsStaticMigrationSqlServer(context, schemaName);
    }
}