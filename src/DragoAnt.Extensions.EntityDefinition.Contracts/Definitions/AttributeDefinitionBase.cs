using System.Reflection;

namespace DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

public abstract class AttributeDefinitionBase<T, TAttr> : MemberInfoDefinition<T> 
    where TAttr : Attribute
{
    private readonly bool _copyParentValueIfUndefined;

    /// <inheritdoc />
    protected AttributeDefinitionBase(string name, bool copyParentValueIfUndefined=false, Func<T, string>? convertToString = null)
        : base(name, convertToString)
    {
        _copyParentValueIfUndefined = copyParentValueIfUndefined;
    }

    /// <inheritdoc />
    public override T? Extract(MemberInfo? member, T? parentValue, EntityDefinitionRow entityRow, PropertyDefinitionRow? row,
        DefinitionContext context)
    {
        TAttr? attr = null;
        if (member is not null)
        {
            attr = member.GetCustomAttribute<TAttr>() ?? GetParentAttribute(member);
        }
        if (attr is { } a)
        {
            return GetValue(a, context);
        }
        return _copyParentValueIfUndefined ? parentValue : default;
    }

    private static TAttr? GetParentAttribute(MemberInfo member)
    {
        return member.MemberType == MemberTypes.TypeInfo ? member.Module.Assembly.GetCustomAttribute<TAttr>() : null;
    }
        
    protected abstract T? GetValue(TAttr attr, DefinitionContext context);
}