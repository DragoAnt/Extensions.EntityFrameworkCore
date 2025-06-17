using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.InMemory.DependencyInjection;

public class EnumsStaticMigrationFactoryInMemory : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EmptyStaticSqlMigration();
    }
}