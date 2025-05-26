using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Table;

namespace DragoAnt.EntityDefinition.Writer;

public interface IDefinitionColumn
{
    DefinitionInfo Info { get; }
    string? ColumnName { get; }
    DefinitionColumnType ColumnType { get; }
    Func<object?, string?>? ConvertToStringFunc { get; }
}