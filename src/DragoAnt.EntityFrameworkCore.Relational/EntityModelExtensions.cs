using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragoAnt.EntityFrameworkCore.Relational;

public static class EntityModelExtensions
{
    public static string GetFinalColumnName(this IProperty property)
#if NET7_0
        => property.GetFinalColumnName(GetIdentifier(property.DeclaringEntityType));
#else
        => property.GetFinalColumnName(GetIdentifier(property.DeclaringType));
#endif
    public static StoreObjectIdentifier GetIdentifier(this IEntityType entity)
    {
        var viewName = entity.GetViewName();
        var type = !string.IsNullOrEmpty(viewName) ? StoreObjectType.View : StoreObjectType.Table;
        return StoreObjectIdentifier.Create(entity, type)!.Value;
    }

#if NET8_0_OR_GREATER
    public static StoreObjectIdentifier GetIdentifier(this ITypeBase entity)
    {
        var viewName = entity.GetViewName();
        var type = !string.IsNullOrEmpty(viewName) ? StoreObjectType.View : StoreObjectType.Table;
        return StoreObjectIdentifier.Create(entity, type)!.Value;
    }
#endif

    public static string GetFinalColumnName(this IProperty property, StoreObjectIdentifier identifier) 
        => property.GetColumnName(identifier) ?? property.GetColumnName();
}