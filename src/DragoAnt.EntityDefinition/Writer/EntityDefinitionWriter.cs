using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Table;
using DragoAnt.Shared.Tables;

namespace DragoAnt.EntityDefinition.Writer;

public sealed class EntityDefinitionWriter
{
    private readonly DefinitionWriterOptions _options;

    public EntityDefinitionWriter(DefinitionWriterOptions options)
    {
        _options = options;
    }

    private IEnumerable<EntityDefinitionWriterColumn> GetColumns(IDefinitionMap map)
    {
        foreach (var column in _options.Columns)
        {
            var columnName = column.ColumnName ?? column.Info.Name;
            switch (column.ColumnType)
            {
                case DefinitionColumnType.Entity:
                    if (!map.EntityDefinitions.Contains(column.Info))
                    {
                        continue;
                    }
                    break;
                case DefinitionColumnType.Property:
                    if (!map.PropertyDefinitions.Contains(column.Info))
                    {
                        continue;
                    }
                    if (column.ColumnName is null && map.EntityDefinitions.Contains(column.Info))
                    {
                        columnName = $"Property:{columnName}";
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield return new EntityDefinitionWriterColumn(new TableWriterColumn(columnName, column.Info.ValueType), column, GetConvertToStringFunc(column));
        }
    }

    private Func<object?, string?> GetConvertToStringFunc(IDefinitionColumn column)
    {
        return column.ConvertToStringFunc ??
               _options.CommonConverts.FirstOrDefault(c => c.Type == column.Info.ValueType)?.ConvertToString
               ?? column.Info.ConvertToString;
    }

    public void Write(IDefinitionMap map, ITableWriter<object?> writer)
    {
        Write(map, writer.ToDefinitionTableWriter((val, _) => val));
    }

    public void Write(IDefinitionMap map, ITableWriter<string?> writer)
    {
        Write(map, writer.ToDefinitionTableWriter((val, c) => c.ConvertToString(val)));
    }

    public EntityDefinitionTable Write(IDefinitionMap map)
    {
        var columns = GetColumns(map).ToList();
        var writer = new EntityDefinitionTableWriter();
        Write(columns, map, writer);
        return writer.ToTable();
    }

    public void Write(IDefinitionMap map, IEntityDefinitionTableWriter writer)
    {
        var columns = GetColumns(map).ToList();
        Write(columns, map, writer);
    }

    private static void Write(IReadOnlyList<EntityDefinitionWriterColumn> columns, IDefinitionMap map, IEntityDefinitionTableWriter writer)
    {
        writer.SetColumns(columns);
        if (columns.All(c => c.Column.ColumnType != DefinitionColumnType.Property))
        {
            foreach (var entity in map.Entities)
            {
                var row = new object?[columns.Count];
                for (var i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];
                    row[i] = entity.Get(column.Column.Info);
                }
                writer.WriteRow(row);
            }
        }
        else
        {
            foreach (var entity in map.Entities)
            {
                foreach (var property in entity.Properties)
                {
                    var row = new object?[columns.Count];
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var column = columns[i];
                        DefinitionRowBase definitionRow = column.Column.ColumnType switch
                        {
                            DefinitionColumnType.Entity => entity,
                            DefinitionColumnType.Property => property,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        row[i] = definitionRow.Get(column.Column.Info);
                    }
                    writer.WriteRow(row);
                }
            }
        }
    }
}