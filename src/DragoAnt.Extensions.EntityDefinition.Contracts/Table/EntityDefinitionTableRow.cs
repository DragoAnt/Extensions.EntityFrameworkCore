using System.Diagnostics;

namespace DragoAnt.Extensions.EntityDefinition.Contracts.Table;

[DebuggerTypeProxy(typeof(DebuggerView))]
public sealed class EntityDefinitionTableRow
{
    private readonly EntityDefinitionTable _table;
    private readonly object?[] _row;

    public EntityDefinitionTableRow(EntityDefinitionTable table, object?[] row)
    {
        _table = table;
        _row = row;
    }

    public object? this[int index] => _row[index];

    public T? Get<T>(DefinitionInfo<T> info, DefinitionColumnType type)
    {
        return (T?)Get((DefinitionInfo)info, type);
    }

    public object? Get(DefinitionInfo info, DefinitionColumnType type)
    {
        var index = _table.GetIndexOfColumn(info, type);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(info), "Can't find column");
        }
        return _row[index];
    }

    public T? GetValueOrDefault<T>(DefinitionInfo<T> info, DefinitionColumnType type)
    {
        return (T?)GetValueOrDefault((DefinitionInfo)info, type);
    }

    public object? GetValueOrDefault(DefinitionInfo info, DefinitionColumnType type)
    {
        var index = _table.GetIndexOfColumn(info, type);
        return index < 0 ? null : _row[index];
    }

    public string? GetString(DefinitionInfo info, DefinitionColumnType type)
    {
        var index = _table.GetIndexOfColumn(info, type);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(info), "Can't find column");
        }
        var column = _table.Columns[index];

        return column.ConvertToStringFunc(_row[index]);
    }

    private sealed class DebuggerView
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EDTRow[]? _items;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EntityDefinitionTableRow _row;

        public DebuggerView(EntityDefinitionTableRow row)
        {
            _row = row;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public EDTRow[] Items =>
            _items ??= _row._row.Select((v, i) => new EDTRow(v, _row._table.Columns[i].ColumnName)).ToArray();
    }
}

/// <summary>
/// Debugger view item for <see cref="EntityDefinitionTableRow"/>
/// </summary>
internal sealed class EDTRow
{
    public EDTRow(object? v, string clmn)
    {
        Clmn = clmn;
        V = v;
    }

    public string Clmn { get; }
    public object? V { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        var val = V is null ? "<null>" : V.ToString();
        return $"{Clmn}: {val}";
    }
}