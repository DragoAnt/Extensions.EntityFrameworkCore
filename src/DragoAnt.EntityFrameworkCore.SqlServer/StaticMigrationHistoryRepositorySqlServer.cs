//Based on: SqlServerHistoryRepository
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;

namespace DragoAnt.EntityFrameworkCore.SqlServer;

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
public class StaticMigrationHistoryRepositorySqlServer : StaticMigrationHistoryRepository
{
    public StaticMigrationHistoryRepositorySqlServer(HistoryRepositoryDependencies dependencies)
        : base(dependencies)
    {
    }

    protected override string ExistsSql
    {
        get
        {
            var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
            return "SELECT OBJECT_ID(" + stringTypeMapping.GenerateSqlLiteral(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)) + ")"
                   + SqlGenerationHelper.StatementTerminator;
        }
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    protected override bool InterpretExistsResult(object? value)
    {
        return value != null && value != DBNull.Value;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public override string GetCreateIfNotExistsScript()
    {
        var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));

        var builder = new StringBuilder()
            .Append("IF OBJECT_ID(")
            .Append(stringTypeMapping.GenerateSqlLiteral(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)))
            .AppendLine(") IS NULL")
            .AppendLine("BEGIN");

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
                    builder.Append("    ").Append(line);
                }
            }
        }

        builder.AppendLine().Append("END")
            .AppendLine(SqlGenerationHelper.StatementTerminator);

        return builder.ToString();
    }
}