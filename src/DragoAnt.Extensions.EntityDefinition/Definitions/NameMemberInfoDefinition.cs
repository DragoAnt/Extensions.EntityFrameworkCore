using System.Reflection;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Definitions;

internal sealed class NameMemberInfoDefinition : MemberInfoDefinition<string>
{
    /// <inheritdoc />
    public NameMemberInfoDefinition()
        : base("Name")
    {
    }

    /// <inheritdoc />
    public override string? Extract(MemberInfo? member, string? parentValue, EntityDefinitionRow entityRow, PropertyDefinitionRow? row,
        DefinitionContext context)
    {
        return member?.Name;
    }
}