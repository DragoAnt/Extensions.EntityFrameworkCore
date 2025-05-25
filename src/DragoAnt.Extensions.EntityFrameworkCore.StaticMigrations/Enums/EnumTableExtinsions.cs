using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityFrameworkCore.Relational;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

public static class EnumTableExtinsions
{
    public static IEnumerable<ModelEnumTable> ExtractEnumTables(this IModel model)
    {
        foreach (var group in model.GetEntityTypes().Where(e => !e.IsView()).SelectMany(e => e.GetProperties())
                     .Distinct() // For abstract entity and inheritors: Proprty will appears count of inheritors
                     .Where(p => GetPropertyType(p).IsEnum).GroupBy(GetPropertyType))
        {
            var table = EnumTable.FromEnum(group.Key);
            yield return new ModelEnumTable(table, group.ToArray());
        }
    }

    private static Type GetPropertyType(IProperty p)
    {
        var type = p.ClrType;
        if (type.IsEnum)
        {
            return type;
        }
        if (type.IsValueType && Nullable.GetUnderlyingType(type) is { } nullableType)
        {
            return nullableType;
        }
        return type;
    }
}