using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using DragoAnt.Extensions.EntityFrameworkCore.Relational;
using DragoAnt.Extensions.StaticMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

public abstract class EnumsStaticMigration : StaticMigration, IStaticSqlMigration
{
    protected const string KeyColumnName = "Id";

    protected readonly DbContext Context;
    protected readonly string SchemaName;

    public EnumsStaticMigration(DbContext context, string schemaName = "enum")
    {
        Context = context;
        SchemaName = schemaName;
    }

    /// <inheritdoc />
    protected override byte[] GetHashInternal()
    {
        var items = GetEnumTables().OrderBy(x => x.Table.TableName).Select(x => new
        {
            Schema = SchemaName,
            Table = new
            {
                Name = x.Table.TableName,
                EnumType = x.Table.EnumType.FullName,
                ValueType = x.Table.ValueType.FullName,
                x.Table.Rows
            },
            Properties = x.Properties.Select(p => new
                {
                    Property = p,
#if NET7_0
                    Parent = p.DeclaringEntityType.GetIdentifier()
#else
                    Parent = p.DeclaringType.GetIdentifier()
#endif
                })
                .OrderBy(t => t.Parent).ThenBy(p => p.Property.Name)
                .Select(t => new
                {
                    t.Parent,
                    t.Property.Name
                })
        });
        return GetHash(items);
    }

    /// <inheritdoc />
    public abstract IEnumerable<MigrationOperation> GetRevertOperations();

    /// <inheritdoc />
    public virtual IEnumerable<MigrationOperation> GetApplyOperations()
    {
        var enumTables = GetEnumTables();
        var duplicates = enumTables.GroupBy(t => t.Table.TableName).Where(g => g.Count() > 1).ToList();
        if (duplicates.Count > 0)
        {
            var duplicatesStr = string.Join(", ", duplicates.Select(g => g.Key));
            throw new EnumStaticMigrationException($"Enum tables duplicates:'{duplicatesStr}'. Use EnumTableAttribute to specify different name");
        }

        if (NeedCreateSchema)
        {
            yield return new EnsureSchemaOperation
            {
                Name = SchemaName
            };
        }

        foreach (var enumTable in enumTables)
        {
            #region Create table

            var tableOp = GetCreateTableOperation(enumTable);
            var table = StoreObjectIdentifier.Table(tableOp.Name, tableOp.Schema);

            var prop = enumTable.Properties.First();
            //TODO: Check that all properties have the same type

#if NET7_0
            var tableQuilifier = prop.DeclaringEntityType.GetIdentifier();
#else
            var tableQuilifier = prop.DeclaringType.GetIdentifier();
#endif

            tableOp.Columns.Add(new AddColumnOperation
            {
                Schema = table.Schema,
                Table = table.Name,
                Name = KeyColumnName,
                ClrType = prop.ClrType,
                ColumnType = prop.GetColumnType(tableQuilifier),
                Scale = prop.GetScale(tableQuilifier),
                Precision = prop.GetPrecision(tableQuilifier),
                MaxLength = prop.GetMaxLength(tableQuilifier),
                IsNullable = false,
                IsUnicode = prop.IsUnicode(tableQuilifier),
                IsFixedLength = prop.IsFixedLength(tableQuilifier)
            });

            tableOp.Columns.Add(new AddColumnOperation
            {
                Schema = table.Schema,
                Table = table.Name,
                Name = "Name",
                ClrType = typeof(string),
                MaxLength = 256,
                IsNullable = false,
                IsUnicode = true,
                IsFixedLength = false
            });
            tableOp.Columns.Add(new AddColumnOperation
            {
                Schema = table.Schema,
                Table = table.Name,
                Name = "DisplayName",
                ClrType = typeof(string),
                MaxLength = 256,
                IsNullable = false,
                IsUnicode = true,
                IsFixedLength = false
            });
            tableOp.Columns.Add(new AddColumnOperation
            {
                Schema = table.Schema,
                Table = table.Name,
                Name = "Description",
                ClrType = typeof(string),
                MaxLength = 256,
                IsNullable = false,
                IsUnicode = true,
                IsFixedLength = false
            });

            tableOp.PrimaryKey = new AddPrimaryKeyOperation
            {
                Schema = table.Schema,
                Table = table.Name,
                Name = $"PK_{table.Name}",
                Columns = [KeyColumnName]
            };
            yield return tableOp;

            #endregion

            #region Insert data

            if (enumTable.Table.Rows.Count != 0)
            {
                var insertOp = new InsertDataOperation
                {
                    Schema = table.Schema,
                    Table = table.Name,
                    Columns = tableOp.Columns.Select(c => c.Name).ToArray(),
                    ColumnTypes = tableOp.Columns.Select(c => c.ColumnType!).ToArray()
                };

                var convert = prop.GetValueConverter()?.ConvertToProvider;
                var providerClrType = prop.GetProviderClrType();


                var values = new object?[enumTable.Table.Rows.Count, 4];
                for (var i = 0; i < enumTable.Table.Rows.Count; i++)
                {
                    var row = enumTable.Table.Rows[i];
                    values[i, 0] = convert != null
                        ? convert(row.RawValue)
                        : providerClrType != null
                            ? Convert.ChangeType(row.RawValue, providerClrType)
                            : row.Value;
                    values[i, 1] = row.Name;
                    values[i, 2] = row.DisplayName;
                    values[i, 3] = row.Description;
                }

                insertOp.Values = values;
                yield return insertOp;
            }

            #endregion

            #region Foreign keys

            foreach (var property in enumTable.Properties)
            {
#if NET7_0
                var identifier = property.DeclaringEntityType.GetIdentifier();
#else
                var identifier = property.DeclaringType.GetIdentifier();
#endif
                var columnName = property.GetFinalColumnName();

                yield return new AddForeignKeyOperation
                {
                    Name = GetFKName(identifier, table, columnName),
                    Schema = identifier.Schema,
                    Table = identifier.Name,
                    PrincipalSchema = table.Schema,
                    PrincipalTable = table.Name,
                    Columns = [columnName],
                    PrincipalColumns = [KeyColumnName],
                    OnDelete = ReferentialAction.NoAction,
                    OnUpdate = ReferentialAction.NoAction
                };
            }

            #endregion
        }
    }

    protected bool NeedCreateSchema => true;

    protected virtual string GetFKName(StoreObjectIdentifier hostTable, StoreObjectIdentifier enumTable, string columnName)
    {
        return $"FK_{hostTable.Name}_{enumTable.Schema}_{enumTable.Name}_{columnName}";
    }

    protected virtual CreateTableOperation GetCreateTableOperation(ModelEnumTable enumTable)
    {
        var tableOp = new CreateTableOperation
        {
            Schema = SchemaName,
            Name = enumTable.Table.TableName
        };
        return tableOp;
    }


    private List<ModelEnumTable> GetEnumTables()
    {
        return Context.Model.ExtractEnumTables().ToList();
    }
}