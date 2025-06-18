using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;

namespace DragoAnt.EntityFrameworkCore.PostgreSQL;

/// <summary>
///     <para>
///         This is an internal API that supports the Entity Framework Core infrastructure and not subject to
///         the same compatibility standards as public APIs.
///     </para>
///     <para>
///         The service lifetime is <see cref="ServiceLifetime.Transient" />.
///         The implementation may depend on other services registered with any lifetime.
///         The implementation does not need to be thread-safe.
///     </para>
/// </summary>
public class StaticMigrationHistoryRepositoryPostgreSQL : StaticMigrationHistoryRepository
{
    public StaticMigrationHistoryRepositoryPostgreSQL(HistoryRepositoryDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override string ExistsSql
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append("SELECT EXISTS (");
            AddTableExistsCondition(builder);
            builder.Append($"){SqlGenerationHelper.StatementTerminator}");
            return builder.ToString();
        }
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    protected override bool InterpretExistsResult(object? value)
        => value != null && value != DBNull.Value && (bool)value;

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public override string GetCreateIfNotExistsScript()
    {
        var builder = new StringBuilder()
            .AppendLine("DO $$")
            .AppendLine("BEGIN")
            .Append("    IF NOT EXISTS (");
        AddTableExistsCondition(builder);
        builder.AppendLine(") THEN");

        using (var reader = new StringReader(GetCreateScript()))
        {
            var first = true;
            while (reader.ReadLine() is { } line)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendLine();
                }

                if (line.Length != 0)
                {
                    builder.Append("        ").Append(line);
                }
            }
        }

        builder.AppendLine()
            .AppendLine("    END IF;")
            .AppendLine("END $$;");

        return builder.ToString();
    }

    private void AddTableExistsCondition(StringBuilder builder)
    {
        var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));

        var tableNameLiteral = stringTypeMapping.GenerateSqlLiteral(TableName);
        builder.Append(
            $"SELECT 1 FROM information_schema.tables WHERE table_name = {tableNameLiteral}");

        if (TableSchema is { Length: > 0 })
        {
            var tableSchemaLiteral = stringTypeMapping.GenerateSqlLiteral(TableSchema);
            builder.Append($" AND schema_name = {tableSchemaLiteral}");
        }
    }
}