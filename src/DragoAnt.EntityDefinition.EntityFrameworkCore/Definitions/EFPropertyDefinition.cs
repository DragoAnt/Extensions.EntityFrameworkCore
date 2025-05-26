using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.EntityDefinition.EntityFrameworkCore.Definitions;

public class EFPropertyDefinition<T> : Definition<T>, IEFPropertyDefinition<T>
{
    public static implicit operator EFPropertyDefinition<T>(MemberInfoDefinition<T> d) => d.ToProperty();
        
    private readonly Func<IPropertyBase?, PropertyInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow, DefinitionContext, T?> _extract;

    /// <inheritdoc />
    public EFPropertyDefinition(DefinitionInfo<T> info,
        Func<IPropertyBase?, PropertyInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow, DefinitionContext, T?> extract)
        : base(info)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }


    /// <inheritdoc />
    public EFPropertyDefinition(string name,
        Func<IPropertyBase?, PropertyInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow, DefinitionContext, T?> extract,
        Func<T, string>? convertToString = null)
        : base(name, convertToString)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    public T? Extract(IPropertyBase? property, PropertyInfo? propertyInfo, T? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow row, DefinitionContext context)
    {
        return _extract(property, propertyInfo, parentValue, entityRow, row, context);
    }
}