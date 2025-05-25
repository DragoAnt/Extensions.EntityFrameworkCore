//Based on: HistoryRepository
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.StaticMigrations;

/// <summary>
///     <para>
///         A base class for the repository used to access the '__StaticMigrationsHistory' table that tracks metadata
///         about Static Migrations such as which migrations have been applied.
///     </para>
///     <para>
///         Database providers must inherit from this class to implement provider-specific functionality.
///     </para>
///     <para>
///         The service lifetime is <see cref="ServiceLifetime.Transient" />.
///         The implementation may depend on other services registered with any lifetime.
///         The implementation does not need to be thread-safe.
///     </para>
/// </summary>
public abstract class StaticMigrationHistoryRepository : IStaticMigrationHistoryRepository
{
    /// <summary>
    ///     The default name for the Migrations history table.
    /// </summary>
    public const string DefaultTableName = "__StaticMigrationsHistory";
    private IModel? _model;

    /// <summary>
    ///     Initializes a new instance of this class.
    /// </summary>
    /// <param name="dependencies"> Parameter object containing dependencies for this service. </param>
    protected StaticMigrationHistoryRepository(HistoryRepositoryDependencies dependencies)
    {
        Dependencies = dependencies;

        var relationalOptions = RelationalOptionsExtension.Extract(dependencies.Options);
        TableName = DefaultTableName;
        TableSchema = relationalOptions?.MigrationsHistoryTableSchema;
    }

    /// <summary>
    ///     Parameter object containing service dependencies.
    /// </summary>
    protected virtual HistoryRepositoryDependencies Dependencies { get; }

    /// <summary>
    ///     A helper class for generation of SQL.
    /// </summary>
    protected virtual ISqlGenerationHelper SqlGenerationHelper
        => Dependencies.SqlGenerationHelper;

    /// <summary>
    ///     THe history table name.
    /// </summary>
    protected virtual string TableName { get; }

    /// <summary>
    ///     The schema that contains the history table, or <see langword="null" /> if the default schema should be used.
    /// </summary>
    protected virtual string? TableSchema { get; }

    /// <summary>
    ///     The name of the column that holds the Static migration identifier.
    /// </summary>
    protected virtual string MigrationNameColumnName => "Name";

    /// <summary>
    ///     The name of the column that holds the Static migration unique hash.
    /// </summary>
    protected virtual string HashColumnName => "Hash";

    /// <summary>
    ///     The max length of hash column
    /// </summary>
    protected virtual int HashColumnMaxLength => 64;

    /// <summary>
    ///     The name of the column that holds the Static migration last modified.
    /// </summary>
    protected virtual string ModifiedColumnName => "Modified";

    /// <summary>
    ///     Overridden by database providers to generate SQL that tests for existence of the history table.
    /// </summary>
    protected abstract string ExistsSql { get; }

    /// <summary>
    ///     Generates SQL to query for the migrations that have been applied.
    /// </summary>
    protected virtual string GetAppliedMigrationsSql
        => new StringBuilder()
            .Append("SELECT ")
            .Append(SqlGenerationHelper.DelimitIdentifier(MigrationNameColumnName))
            .Append(", ")
            .AppendLine(SqlGenerationHelper.DelimitIdentifier(HashColumnName))
            .Append(", ")
            .AppendLine(SqlGenerationHelper.DelimitIdentifier(ModifiedColumnName))
            .Append("FROM ")
            .AppendLine(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
            .Append("ORDER BY ")
            .Append(SqlGenerationHelper.DelimitIdentifier(MigrationNameColumnName))
            .Append(SqlGenerationHelper.StatementTerminator)
            .ToString();

    private IModel EnsureModel()
    {
        if (_model != null)
        {
            return _model;
        }

        var conventionSet = Dependencies.ConventionSetBuilder.CreateConventionSet();

        // Use public API to remove the convention, issue #214
        ConventionSet.Remove(conventionSet.ModelInitializedConventions, typeof(DbSetFindingConvention));
        ConventionSet.Remove(conventionSet.ModelInitializedConventions, typeof(RelationalDbFunctionAttributeConvention));

        var modelBuilder = new ModelBuilder(conventionSet);
        modelBuilder.Entity<StaticMigrationHistoryRow>(
            x =>
            {
                ConfigureTable(x);
                x.ToTable(TableName, TableSchema);
            });

        _model = Dependencies.ModelRuntimeInitializer.Initialize((IModel)modelBuilder.Model);
        return _model;
    }
        
    /// <summary>
    ///     Checks whether or not the history table exists.
    /// </summary>
    /// <returns> <see langword="true" /> if the table already exists, <see langword="false" /> otherwise. </returns>
    public virtual bool Exists()
    {
        return Dependencies.DatabaseCreator.Exists()
               && InterpretExistsResult(
                   Dependencies.RawSqlCommandBuilder.Build(ExistsSql).ExecuteScalar(
                       new RelationalCommandParameterObject(
                           Dependencies.Connection,
                           null,
                           null,
                           Dependencies.CurrentContext.Context,
                           Dependencies.CommandLogger)));
    }

    /// <summary>
    ///     Checks whether or not the history table exists.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     <see langword="true" /> if the table already exists, <see langword="false" /> otherwise.
    /// </returns>
    public virtual async Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
    {
        var val = await Dependencies.RawSqlCommandBuilder.Build(ExistsSql).ExecuteScalarAsync(
            new RelationalCommandParameterObject(
                Dependencies.Connection,
                null,
                null,
                Dependencies.CurrentContext.Context,
                Dependencies.CommandLogger),
            cancellationToken).ConfigureAwait(false);
            
        return await Dependencies.DatabaseCreator.ExistsAsync(cancellationToken).ConfigureAwait(false)
               && InterpretExistsResult(val);
    }

    /// <summary>
    ///     Interprets the result of executing <see cref="ExistsSql" />.
    /// </summary>
    /// <returns><see langword="true" /> if the table already exists, <see langword="false" /> otherwise.</returns>
    protected abstract bool InterpretExistsResult(object? value);

    /// <summary>
    ///     Overridden by a database provider to generate a SQL script that will create the history table
    ///     if and only if it does not already exist.
    /// </summary>
    /// <returns> The SQL script. </returns>
    public abstract string GetCreateIfNotExistsScript();

    /// <summary>
    ///     Generates a SQL script that will create the history table.
    /// </summary>
    /// <returns> The SQL script. </returns>
    public virtual string GetCreateScript()
    {
        var model = EnsureModel();

        var operations = Dependencies.ModelDiffer.GetDifferences(null, model.GetRelationalModel());
        var commandList = Dependencies.MigrationsSqlGenerator.Generate(operations, model);

        return string.Concat(commandList.Select(c => c.CommandText));
    }

    /// <summary>
    ///     <para>
    ///         Configures the entity type mapped to the history table.
    ///     </para>
    ///     <para>
    ///         Database providers can override this to add or replace configuration.
    ///     </para>
    /// </summary>
    /// <param name="history"> A builder for the <see cref="HistoryRow" /> entity type. </param>
    protected virtual void ConfigureTable(EntityTypeBuilder<StaticMigrationHistoryRow> history)
    {
        history.ToTable(DefaultTableName, TableSchema);
        history.HasKey(h => h.Name).HasName(MigrationNameColumnName);
        //Length for using SHA256 as main hash algorithm
        history.Property(h => h.Hash).HasColumnName(HashColumnName).HasMaxLength(HashColumnMaxLength).IsRequired();
        history.Property(h => h.Modified).HasColumnName(ModifiedColumnName).IsRequired();
    }

    /// <summary>
    ///     Queries the history table for all migrations that have been applied.
    /// </summary>
    /// <returns> The list of applied migrations, as <see cref="StaticMigrationHistoryRow" /> entities. </returns>
    public virtual IReadOnlyList<StaticMigrationHistoryRow> GetAppliedMigrations()
    {
        if (!Exists())
        {
            return [];
        }

        var rows = new List<StaticMigrationHistoryRow>();
        var command = Dependencies.RawSqlCommandBuilder.Build(GetAppliedMigrationsSql);

        using var reader = command.ExecuteReader(
            new RelationalCommandParameterObject(
                Dependencies.Connection,
                null,
                null,
                Dependencies.CurrentContext.Context,
                Dependencies.CommandLogger));
        while (reader.Read())
        {
            rows.Add(CreateRow(reader));
        }

        return rows;
    }

    /// <summary>
    ///     Queries the history table for all migrations that have been applied.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the list of applied migrations, as <see cref="StaticMigrationHistoryRow" /> entities.
    /// </returns>
    public virtual async Task<IReadOnlyList<StaticMigrationHistoryRow>> GetAppliedMigrationsAsync(
        CancellationToken cancellationToken = default)
    {
        if (!await ExistsAsync(cancellationToken).ConfigureAwait(false))
        {
            return [];
        }

        var rows = new List<StaticMigrationHistoryRow>();
        var command = Dependencies.RawSqlCommandBuilder.Build(GetAppliedMigrationsSql);

        await using var reader = await command.ExecuteReaderAsync(
            new RelationalCommandParameterObject(
                Dependencies.Connection,
                null,
                null,
                Dependencies.CurrentContext.Context,
                Dependencies.CommandLogger),
            cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(CreateRow(reader));
        }

        return rows;
    }

    protected StaticMigrationHistoryRow CreateRow(RelationalDataReader reader)
    {
        var name = reader.DbDataReader.GetString(0);

        var hash = new byte[HashColumnMaxLength];
        var hashLen = reader.DbDataReader.GetBytes(1, 0, hash, 0, HashColumnMaxLength);
        Array.Resize(ref hash, (int)hashLen);

        var modified = reader.DbDataReader.GetDateTime(2);
        var row = new StaticMigrationHistoryRow(name, hash, modified);
        return row;
    }

    /// <summary>
    ///     Generates a SQL script to insert a row into the history table.
    /// </summary>
    /// <param name="row"> The row to insert, represented as a <see cref="HistoryRow" /> entity. </param>
    /// <returns> The generated SQL. </returns>
    public virtual string GetInsertScript(StaticMigrationHistoryRow row)
    {
        var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
        var byteArrayTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(byte[]));
        var datetimeTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(DateTime));

        return new StringBuilder().Append("INSERT INTO ")
            .Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
            .Append(" (")
            .Append(SqlGenerationHelper.DelimitIdentifier(MigrationNameColumnName))
            .Append(", ")
            .Append(SqlGenerationHelper.DelimitIdentifier(HashColumnName))
            .Append(", ")
            .Append(SqlGenerationHelper.DelimitIdentifier(ModifiedColumnName))
            .AppendLine(")")
            .Append("VALUES (")
            .Append(stringTypeMapping.GenerateSqlLiteral(row.Name))
            .Append(", ")
            .Append(byteArrayTypeMapping.GenerateSqlLiteral(row.Hash))
            .Append(", ")
            .Append(datetimeTypeMapping.GenerateSqlLiteral(row.Modified))
            .Append(")")
            .AppendLine(SqlGenerationHelper.StatementTerminator)
            .ToString();
    }

    /// <summary>
    ///     Generates a SQL script to delete a row from the history table.
    /// </summary>
    /// <param name="row"> The migration row to delete. </param>
    /// <returns> The generated SQL. </returns>
    public virtual string GetDeleteScript(StaticMigrationHistoryRow row)
    {
        return GetDeleteScript(row.Name);
    }

    /// <summary>
    ///     Generates a SQL script to delete a row from the history table.
    /// </summary>
    /// <param name="name"> The migration identifier of the row to delete. </param>
    /// <returns> The generated SQL. </returns>
    public virtual string GetDeleteScript(string name)
    {
        var stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));

        return new StringBuilder().Append("DELETE FROM ")
            .AppendLine(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
            .Append("WHERE ")
            .Append(SqlGenerationHelper.DelimitIdentifier(MigrationNameColumnName))
            .Append(" = ")
            .Append(stringTypeMapping.GenerateSqlLiteral(name))
            .AppendLine(SqlGenerationHelper.StatementTerminator)
            .ToString();
    }
}