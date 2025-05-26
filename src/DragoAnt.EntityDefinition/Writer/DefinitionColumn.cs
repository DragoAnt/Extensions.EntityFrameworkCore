using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Table;

namespace DragoAnt.EntityDefinition.Writer;

public sealed class DefinitionColumn<T> : IDefinitionColumn
{
    public DefinitionColumn(DefinitionInfo<T> info, DefinitionColumnType columnType, string? columnName, Func<T?, string?>? convertToStringFunc)
    {
        Info = info;
        ColumnType = columnType;
        ColumnName = columnName;

        if (convertToStringFunc is not null)
        {
            ConvertToStringFunc = o => o is null
                // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
                ? convertToStringFunc(default(T?))
                : convertToStringFunc((T?)o);
        }
    }

    /// <inheritdoc />
    DefinitionInfo IDefinitionColumn.Info => Info;

    public DefinitionInfo<T> Info { get; }
    public DefinitionColumnType ColumnType { get; }
    public string? ColumnName { get; }
    /// <inheritdoc />
    public Func<object?, string?>? ConvertToStringFunc { get;  }
}