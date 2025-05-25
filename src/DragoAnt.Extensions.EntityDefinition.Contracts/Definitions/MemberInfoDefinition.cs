using System.Reflection;

namespace DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

public abstract class MemberInfoDefinition<T> : Definition<T>
{
    /// <inheritdoc />
    protected MemberInfoDefinition(DefinitionInfo<T> info) : base(info)
    {
    }

    /// <inheritdoc />
    protected MemberInfoDefinition(string name, Func<T, string>? convertToString = null)
        : base(name, convertToString)
    {
    }

    public abstract T? Extract(MemberInfo? member, T? parentValue, EntityDefinitionRow entityRow, PropertyDefinitionRow? row,
        DefinitionContext context);
}

public sealed class CustomMemberInfoDefinition<T> : MemberInfoDefinition<T>
{
    private readonly Func<MemberInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow?, DefinitionContext, T?> _extract;

    /// <inheritdoc />
    public CustomMemberInfoDefinition(DefinitionInfo<T> info,
        Func<MemberInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow?, DefinitionContext, T?> extract) : base(info)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    /// <inheritdoc />
    public CustomMemberInfoDefinition(string name, Func<MemberInfo?, T?, EntityDefinitionRow, PropertyDefinitionRow?, DefinitionContext, T?> extract,
        Func<T, string>? convertToString = null)
        : base(name, convertToString)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    /// <inheritdoc />
    public override T? Extract(MemberInfo? member, T? parentValue, EntityDefinitionRow entityRow, PropertyDefinitionRow? row,
        DefinitionContext context)
    {
        return _extract(member, parentValue, entityRow, row, context);
    }
}