using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Table;

namespace DragoAnt.Extensions.EntityDefinition.Writer;

public class DefinitionWriterOptions
{
    internal List<IDefinitionColumn> Columns { get; } = new();
    internal List<CommonConvert> CommonConverts { get; } = new();

    public void AddCommonConvert<T>(Func<T?, string?> convertToString)
    {
        CommonConverts.RemoveAll(c => c.Type == typeof(T));
        CommonConverts.Add(new CommonConvert(typeof(T),
            val => val is null
                // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
                ? null
                : convertToString((T?)val)));
    }

    public void AddEntityColumn<T>(DefinitionInfo<T> info, string? columnName = null, Func<T?, string?>? convertToString = null)
    {
        Columns.Add(new DefinitionColumn<T>(info, DefinitionColumnType.Entity, columnName, convertToString));
    }

    public void AddPropertyColumn<T>(DefinitionInfo<T> info, string? columnName = null, Func<T?, string?>? convertToString = null)
    {
        Columns.Add(new DefinitionColumn<T>(info, DefinitionColumnType.Property, columnName, convertToString));
    }
}

internal record CommonConvert(Type Type, Func<object?, string?> ConvertToString);