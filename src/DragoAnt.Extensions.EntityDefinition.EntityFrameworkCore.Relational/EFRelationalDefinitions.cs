using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;
using DragoAnt.Extensions.EntityFrameworkCore.Relational;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Relational;

/// <summary>
/// Common Entity Framework model definitions
/// </summary>
public static class EFRelationalDefinitions
{
    public static class Entities
    {
        public static readonly EFEntityDefinition<string> DbName = new("DbName", (type, _, _) => type.GetTableName() ?? type.GetViewName());

        public static readonly EFEntityDefinition<EntityMappingType> Type = new("Type", (type, _, _) => type.GetEntityType());
            
        public static readonly EFEntityDefinition<bool> IsTable = new("IsTable", (type, _, _) => type.IsTable());

        public static readonly EFEntityDefinition<bool> IsView = new("IsView", (type, _, _) => type.IsView());
    }

    public static class Properties
    {
        public static readonly EFPropertyDefinition<string> ColumnName =
            new EFScalarPropertyDefinition<string>("ColumnName", (property, _, _, _, _, _) => property.GetFinalColumnName());

        public static readonly EFPropertyDefinition<string> DbColumnType =
            new EFScalarPropertyDefinition<string>("ColumnType", (property, _, _, _, _, _) => property.GetColumnType());

        /// <summary>
        ///     <para>
        ///         Checks whether the column will be nullable in the database.
        ///     </para>
        ///     <para>
        ///         This depends on the property itself and also how it is mapped. For example,
        ///         derived non-nullable properties in a TPH type hierarchy will be mapped to nullable columns.
        ///         As well as properties on optional types sharing the same table.
        ///     </para>
        /// </summary>
        public static readonly EFPropertyDefinition<bool> IsColumnNullable =
            new EFScalarPropertyDefinition<bool>("IsColumnNullable", (property, _, _, _, _, _) => property.IsColumnNullable());
            
        /// <summary>
        ///     Is property computed or not
        /// </summary>
        public static readonly EFPropertyDefinition<bool> IsComputed =
            new EFScalarPropertyDefinition<bool>("IsComputed", (property, _, _, _, _, _) => property.IsComputed());
            
        /// <summary>
        ///     Gets sql body for computed property
        /// </summary>
        public static readonly EFPropertyDefinition<string> ComputedSql =
            new EFScalarPropertyDefinition<string>("ComputedSql", (property, _, _, _, _, _) => property.GetComputedColumnSql());
    }
}