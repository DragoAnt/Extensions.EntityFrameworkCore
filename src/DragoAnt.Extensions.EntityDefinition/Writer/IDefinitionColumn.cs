using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Table;

namespace DragoAnt.Extensions.EntityDefinition.Writer;

public interface IDefinitionColumn
{
    DefinitionInfo Info { get; }
    string? ColumnName { get; }
    DefinitionColumnType ColumnType { get; }
    Func<object?, string?>? ConvertToStringFunc { get; }
}