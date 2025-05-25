using System.Reflection;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;
using DragoAnt.Shared.Reflection;

namespace DragoAnt.Extensions.EntityDefinition.Definitions;

public sealed class PropertyClrTypeDefinition : MemberInfoDefinition<TypeInfo>
{
    /// <inheritdoc />
    public PropertyClrTypeDefinition(DefinitionInfo<TypeInfo> info)
        : base(info)
    {
    }

    /// <inheritdoc />
    public PropertyClrTypeDefinition(string name)
        : base(name, ConvertToString)
    {
    }

    private static string ConvertToString(TypeInfo type)
    {
        return type.HumanizeName();
    }

    /// <inheritdoc />
    public override TypeInfo? Extract(MemberInfo? member, TypeInfo? parentValue,
        EntityDefinitionRow entityRow, PropertyDefinitionRow? row, DefinitionContext context)
    {
        if (member is PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetTypeInfo();
        }
        if (member is FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType.GetTypeInfo();
        }
        return default;
    }
}