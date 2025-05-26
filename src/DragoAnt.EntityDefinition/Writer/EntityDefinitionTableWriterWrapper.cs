using DragoAnt.Shared.Tables;

namespace DragoAnt.EntityDefinition.Writer;

internal sealed class EntityDefinitionTableWriterWrapper<T> : IEntityDefinitionTableWriter
{
    private readonly ITableWriter<T> _writer;
    private readonly Func<object?, EntityDefinitionWriterColumn, T> _convert;
    private IReadOnlyList<EntityDefinitionWriterColumn>? _columns;

    public EntityDefinitionTableWriterWrapper(ITableWriter<T> writer, Func<object?, EntityDefinitionWriterColumn, T> convert)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _convert = convert ?? throw new ArgumentNullException(nameof(convert));
    }

    /// <inheritdoc />
    public void SetColumns(IReadOnlyList<EntityDefinitionWriterColumn> columns)
    {
        _columns = columns;
        _writer.SetColumns(columns.Select(c => c.WriterColumn).ToArray());
    }

    /// <inheritdoc />
    public void WriteRow(object?[] values)
    {
        if (_columns is null)
        {
            throw new ApplicationException("Call SetColumns first");
        }

        _writer.WriteRow(values.Select((v, i) => _convert(v, _columns[i])).ToArray());
    }
}