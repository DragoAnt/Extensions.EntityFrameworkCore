using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DragoAnt.EntityFrameworkCore.Relational;

public static class EntityTypeExtensions
{
    /// <summary>
    /// Is entity type mapped to View
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsView(this IReadOnlyEntityType entity)
    {
        return entity.GetEntityType() == EntityMappingType.View;
    }

    /// <summary>
    /// Is entity type mapped to Sql Query
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsSqlQuery(this IReadOnlyEntityType entity)
    {
        return entity.GetEntityType() == EntityMappingType.SqlQuery;
    }

    /// <summary>
    /// Is entity type mapped to Sql Query
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsInMemoryQuery(this IReadOnlyEntityType entity)
    {
        return entity.GetEntityType() == EntityMappingType.InMemoryQuery;
    }

    /// <summary>
    /// Is entity type mapped to Sql Query
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsFunction(this IReadOnlyEntityType entity)
    {
        return entity.GetEntityType() == EntityMappingType.Function;
    }

    /// <summary>
    /// Is entity type mapped to Table
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsTable(this IReadOnlyEntityType entity)
    {
        return entity.GetEntityType() == EntityMappingType.Table;
    }

    public static bool IsComputed(this IReadOnlyProperty property)
    {
        return property.FindAnnotation(RelationalAnnotationNames.ComputedColumnSql) != null;
    }

    public static EntityMappingType GetEntityType(this IReadOnlyEntityType entity)
    {
        if (entity.FindAnnotation(RelationalAnnotationNames.TableName)?.Value != null ||
            entity.FindAnnotation(RelationalAnnotationNames.TableMappings) != null)
        {
            return EntityMappingType.Table;
        }
        if (entity.FindAnnotation(RelationalAnnotationNames.ViewName)?.Value != null ||
            entity.FindAnnotation(RelationalAnnotationNames.ViewMappings) != null ||
            entity.FindAnnotation(RelationalAnnotationNames.ViewDefinitionSql) != null)
        {
            return EntityMappingType.View;
        }
        if (entity.FindAnnotation(RelationalAnnotationNames.FunctionName)?.Value != null ||
            entity.FindAnnotation(RelationalAnnotationNames.FunctionMappings) != null)
        {
            return EntityMappingType.Function;
        }
        if (entity.FindAnnotation(RelationalAnnotationNames.SqlQuery) != null)
        {
            return EntityMappingType.SqlQuery;
        }
#pragma warning disable CS0612
        if (entity.FindAnnotation(CoreAnnotationNames.DefiningQuery) != null)
#pragma warning restore CS0612
        {
            return EntityMappingType.InMemoryQuery;
        }

        if (entity.BaseType is { } baseType)
        {
            return baseType.GetEntityType();
        }
        return EntityMappingType.Table;
    }
}