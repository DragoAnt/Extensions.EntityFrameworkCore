using System.Collections;
using System.Diagnostics;

namespace DragoAnt.EntityDefinition.Contracts.Table;

[DebuggerDisplay("Columns:{_columns.Length}, Rows:{RowCount}")]
public class EntityDefinitionTable : IEnumerable<EntityDefinitionTableRow>
{
    private readonly EntityDefinitionTableColumn[] _columns;
    private readonly List<EntityDefinitionTableRow> _rows;

    public EntityDefinitionTable(EntityDefinitionTableColumn[] columns, int rowsCount = 0)
    {
        if (columns == null)
        {
            throw new ArgumentNullException(nameof(columns));
        }
        if (columns.Length == 0)
        {
            throw new ArgumentException("Value cannot be an empty collection.", nameof(columns));
        }
        _columns = columns;
        _rows = new List<EntityDefinitionTableRow>(rowsCount);
    }

    public IReadOnlyList<EntityDefinitionTableColumn> Columns => _columns;

    public int RowCount => _rows.Count;

    public void AddRow(object?[] row)
    {
        if (row == null)
        {
            throw new ArgumentNullException(nameof(row));
        }
        if (row.Length != _columns.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "Row has a  different count of values then table columns");
        }
        _rows.Add(new EntityDefinitionTableRow(this, row));
    }

    public bool RemoveRow(EntityDefinitionTableRow row)
    {
        return _rows.Remove(row);
    }

    public int GetIndexOfColumn(DefinitionInfo info, DefinitionColumnType columnType)
    {
        for (var i = 0; i < _columns.Length; i++)
        {
            var column = _columns[i];
            if (column.Info == info && column.ColumnType == columnType)
            {
                return i;
            }
        }
        return -1;
    }

    /// <inheritdoc />
    public IEnumerator<EntityDefinitionTableRow> GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}