using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

public static class StaticMigrationBuilderExtensions
{
    /// <summary>
    /// Add support for enum tables. All enums used in enity model will be extracted as a tables to database
    /// and referenced to depend columns via foreign keys
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <param name="enumSchemaName"></param>
    /// <exception cref="EnumStaticMigrationException"></exception>
    public static void AddEnumTables(this StaticMigrationBuilder builder, string name = "#Enums", string enumSchemaName = "enum")
    {
        builder.AddStaticSqlFactory(name,
            context =>
            {
                var factory = context.GetInfrastructure().GetService<IEnumsStaticMigrationFactory>();
                if (factory == null)
                {
                    throw new EnumStaticMigrationException("Db provider doesn't support enum tables");
                }
                return factory.Create(context, enumSchemaName);
            });
    }
}