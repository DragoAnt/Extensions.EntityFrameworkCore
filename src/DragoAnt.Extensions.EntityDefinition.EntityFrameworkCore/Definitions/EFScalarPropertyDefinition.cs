using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

public sealed class EFScalarPropertyDefinition<T> : EFPropertyDefinition<T>
{
    /// <inheritdoc />
    public EFScalarPropertyDefinition(string name,
        Func<IProperty, PropertyInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow, DefinitionContext, T?> extract,
        Func<T, string>? convertToString = null)
        : base(name,
            (property, propertyInfo, parentValue, entityRow, row, context) =>
            {
                if (property is IProperty p)
                {
                    return extract(p, propertyInfo, parentValue, entityRow, row, context);
                }
                return default;
            }, convertToString)
    {
        if (extract == null)
        {
            throw new ArgumentNullException(nameof(extract));
        }
    }
}