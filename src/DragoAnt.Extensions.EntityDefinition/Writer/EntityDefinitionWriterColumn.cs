using DragoAnt.Shared.Tables;

namespace DragoAnt.Extensions.EntityDefinition.Writer;

public record EntityDefinitionWriterColumn(TableWriterColumn WriterColumn, IDefinitionColumn Column, Func<object?, string?> ConvertToString);