using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

public interface IEnumsStaticMigrationFactory
{
    IStaticSqlMigration Create(DbContext context, string schemaName = "enum");
}