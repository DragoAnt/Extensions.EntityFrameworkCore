using Microsoft.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

public interface IEnumsStaticMigrationFactory
{
    IStaticSqlMigration Create(DbContext context, string schemaName = "enum");
}