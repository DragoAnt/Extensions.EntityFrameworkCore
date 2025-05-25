using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

public static class EFDefinitionExtensions
{
    public static EFEntityDefinition<T> ToEntity<T>(this MemberInfoDefinition<T> definition)
    {
        return new EFEntityDefinition<T>(definition.Info,
            (type, entityRow, context) => definition.Extract(type.ClrType, default, entityRow, null, context));
    }

    public static EFPropertyDefinition<T> ToProperty<T>(this MemberInfoDefinition<T> info)
    {
        return new EFPropertyDefinition<T>(info.Info,
            (_, propertyInfo, parentValue, entityRow, row, context) => info.Extract(propertyInfo, parentValue, entityRow, row, context));
    }
}