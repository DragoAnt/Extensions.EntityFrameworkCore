namespace DragoAnt.EntityDefinition.Writer;

public interface IEntityDefinitionTableWriter
{
    void SetColumns(IReadOnlyList<EntityDefinitionWriterColumn> columns);
    void WriteRow(object?[] values);
}