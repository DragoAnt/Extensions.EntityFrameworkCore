using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

[DebuggerDisplay("{Table.TableName}")]
public class ModelEnumTable
{
    public ModelEnumTable(EnumTable table, IProperty[] properties)
    {
        Table = table ?? throw new ArgumentNullException(nameof(table));
        Properties = properties ?? throw new ArgumentNullException(nameof(properties));
    }

    public EnumTable Table { get; }
    public IProperty[] Properties { get; }
}