using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public static class MigrationBuilderExtensions
{
    /// <summary>
    /// Drop table if exists
    /// Usually usage for drop temporary table.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="table"></param>
    /// <param name="schema"></param>
    public static void DropTableIfExists(this MigrationBuilder builder, string table, string? schema = null)
    {
        schema ??= GetProviderDefaultSchema(builder);
            
        switch (builder.ActiveProvider)
        {
            case "Microsoft.EntityFrameworkCore.SqlServer":
                builder.Sql($@"DROP TABLE IF EXISTS [{schema}].[{table}]");
                break;

            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                throw new NotImplementedException("");
            default:
                throw new StaticMigrationException($"Unexpected provider for DropTableIfExists. Provider:{builder.ActiveProvider}");
        }
    }

    /// <summary>
    /// Drop DEFAULT constraint for specific table's column if it exists.
    /// Usually usage for drop temporary DEFAULT after creating NOT NULL column on existed table with data.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="table"></param>
    /// <param name="column"></param>
    /// <param name="schema"></param>
    public static void DropDefaultConstraint(this MigrationBuilder builder, string table, string column, string? schema = null)
    {
        schema ??= GetProviderDefaultSchema(builder);

        switch (builder.ActiveProvider)
        {
            case "Microsoft.EntityFrameworkCore.SqlServer":
                builder.Sql($@"DECLARE @Command nvarchar(1000)

    SELECT @Command = 'ALTER TABLE [{schema}].[{table}] DROP CONSTRAINT ' + d.name
    FROM sys.tables              t
             JOIN sys.default_constraints d ON d.parent_object_id = t.object_id
             JOIN sys.columns             c ON c.object_id = t.object_id AND c.column_id = d.parent_column_id
    WHERE t.name = N'{table}'
      AND t.schema_id = SCHEMA_ID(N'{schema}')
      AND c.name = N'{column}'

    IF (LEN(@Command) > 0)
        EXECUTE (@Command)
    ELSE
        PRINT 'Can''t find DEFAULT for column [{column}] in table [{schema}].[{table}]';");
                break;

            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                throw new NotImplementedException("");
            default:
                throw new StaticMigrationException($"Unexpected provider for GetProviderDefaultSchema. Provider:{builder.ActiveProvider}");
        }
    }

    /// <summary>
    /// Drop DEFAULT constraint for specific alter column operation if it exists.
    /// Usually usage for drop temporary DEFAULT after creating NOT NULL column on existed table with data.
    /// </summary>
    /// <param name="opBuilder"></param>
    /// <param name="builder"></param>
    public static void DropDefaultConstraint<TOperation>(this OperationBuilder<TOperation> opBuilder, MigrationBuilder builder)
        where TOperation:ColumnOperation
    {
        var operation = opBuilder.GetInfrastructure();
        builder.DropDefaultConstraint(operation.Table,operation.Name,operation.Schema);
    }
        
    private static string? GetProviderDefaultSchema(this MigrationBuilder builder)
    {
        return builder.ActiveProvider switch
        {
            "Microsoft.EntityFrameworkCore.SqlServer" => "dbo",
            "Npgsql.EntityFrameworkCore.PostgreSQL" => "dbo",
            _ => throw new StaticMigrationException($"Unexpected provider for GetProviderDefaultSchema. Provider:{builder.ActiveProvider}")
        };
    }
}