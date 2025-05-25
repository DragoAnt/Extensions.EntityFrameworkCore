using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

public interface IEFPropertyDefinition : IDefinition
{
    object? Extract(IPropertyBase? property, PropertyInfo? propertyInfo, object? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow row, DefinitionContext context);
}

public interface IEFPropertyDefinition<T> : IEFPropertyDefinition
{
    object? IEFPropertyDefinition.Extract(IPropertyBase? property, PropertyInfo? propertyInfo, object? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow row, DefinitionContext context)
    {
        return parentValue is null
            // ReSharper disable once ArrangeDefaultValueWhenTypeNotEvident
            ? Extract(property, propertyInfo, default(T?), entityRow, row, context)
            : Extract(property, propertyInfo, (T?)parentValue, entityRow, row, context);
    }

    T? Extract(IPropertyBase? property, PropertyInfo? propertyInfo, T? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow row, DefinitionContext context);
}