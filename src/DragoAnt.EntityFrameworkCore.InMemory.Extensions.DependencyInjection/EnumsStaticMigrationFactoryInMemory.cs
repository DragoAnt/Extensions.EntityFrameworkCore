using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.StaticMigrations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.InMemory.Extensions.DependencyInjection;

public class EnumsStaticMigrationFactoryInMemory : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EmptyStaticSqlMigration();
    }
}