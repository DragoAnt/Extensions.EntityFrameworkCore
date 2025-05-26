using System.Diagnostics;
using System.Reflection;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

[DebuggerDisplay("{TableName}, ClrType:{EnumType.Name}, ValueType: {ValueType.Name}, Rows:{Rows.Count}")]
public class EnumTable
{
    public static EnumTable Create<T>()
        where T : struct, Enum
    {
        var enumType = typeof(T);
        var enumTableAttribute = enumType.GetCustomAttribute<EnumTableAttribute>();
        var tableName = enumTableAttribute?.Name ?? enumType.Name;
        var valueType = enumTableAttribute?.ValueType ?? enumType.GetEnumUnderlyingType();

        var enumMembers = enumType.GetMembers(BindingFlags.Static | BindingFlags.Public);

        var rows = new List<EnumTableRow>(enumMembers.Length);
        rows.AddRange(enumMembers.Select(member => EnumTableRow.Create(enumType, valueType, member)));

        if (Attribute.IsDefined(enumType, typeof(FlagsAttribute)))
        {
            AddValueCombinations<T>(valueType, rows);
        }

        rows.Sort((one, other) => string.Compare(one.Name, other.Name, StringComparison.Ordinal));
        return new EnumTable(tableName, enumType, valueType, rows);
    }

    public static EnumTable FromEnum(Type enumType)
    {
        if (enumType == null)
        {
            throw new ArgumentNullException(nameof(enumType));
        }
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type must be enum", nameof(enumType));
        }

        var methodInfo = typeof(EnumTable).GetMethod(nameof(Create), BindingFlags.Public | BindingFlags.Static);
        var createMethod = methodInfo!.MakeGenericMethod(enumType);

        return (EnumTable)createMethod.Invoke(null, null)!;
    }

    private static void AddValueCombinations<T>(Type valueType, List<EnumTableRow> rows) 
        where T : struct, Enum
    {
        var combos = GetEnumValueCombinations<T>();
        rows.AddRange(combos.Select(item => EnumTableRow.CreateFromCombinedEnumValue(valueType, item)));
    }

    /// <summary>
    /// Based on https://stackoverflow.com/a/6117208
    /// First, grab a list of all the individual values. If you've got 5 values, that's (1 *left shift* 5) = 32 combinations, so iterate from 1 to 31.
    /// (Don't start at zero, that would mean to include none of the enum values.) 
    /// When iterating, examine the bits in the number, every one bit in the iteration variable means to include that enum value.
    /// Put the results into a HashSet, so that there aren't duplicates, since including the 'None' value doesn't change the resulting enum.
    /// </summary>
    private static HashSet<T> GetEnumValueCombinations<T>() 
        where T : struct, Enum
    {
        var allValues = Enum.GetValues<T>();
        HashSet<T> result = new();

        for (var i = 1; i < (1 << allValues.Length); i++)
        {
            T working = default;
            var index = 0;
            var checker = i;
            while (checker != 0)
            {
                if ((checker & 0x01) == 0x01)
                {
                    working = (T)Enum.ToObject(typeof(T), Convert.ToUInt64(working) | Convert.ToUInt64(allValues[index]));
                }

                checker >>= 1;
                index++;
            }
            if (allValues.Contains(working))
            {
                continue;
            }
            result.Add(working);
        }

        return result;
    }
        
    private EnumTable(string tableName, Type enumType, Type valueType, IReadOnlyList<EnumTableRow> rows)
    {
        TableName = tableName;
        EnumType = enumType;
        ValueType = valueType;
        Rows = rows;
    }

    public string TableName { get; }

    public Type ValueType { get; }

    public Type EnumType { get; }

    public IReadOnlyList<EnumTableRow> Rows { get; }
}