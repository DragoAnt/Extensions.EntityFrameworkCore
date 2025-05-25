using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.Extensions.EntityFrameworkCore.InMemory.Extensions.DependencyInjection;

public class EnumsStaticMigrationFactoryInMemory : IEnumsStaticMigrationFactory
{
    /// <inheritdoc />
    public IStaticSqlMigration Create(DbContext context, string schemaName = "enum")
    {
        return new EmptyStaticSqlMigration();
    }
}