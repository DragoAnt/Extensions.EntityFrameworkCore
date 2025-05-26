using System.Reflection;
using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.EntityDefinition.Definitions;

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