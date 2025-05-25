using System.Reflection;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model.Definitions;

public static class CustomDefinitions
{
    /// <summary>
    /// Name definition based on <see cref="MemberInfo.Name"/>
    /// </summary>
    public static readonly MemberInfoDefinition<Domain> Domain =
        new AttributeDefinition<Domain, DefinitionDomainAttribute>("Domain", true) { ReadOrder = 0 };

    /// <summary>
    /// Is entity's domain and property's domain are different
    /// </summary>
    public static readonly MemberInfoDefinition<bool> IsDomainDifferent =
        new CustomMemberInfoDefinition<bool>("IsDomainDifferent", (_, _, entityRow, row, _) =>
        {
            if (row == null)
            {
                throw new NotSupportedException("Can't use IsDomainDifferent with entity");
            }
            if (!entityRow.TryGetValue(Domain, out var entityDomain))
            {
                throw new NotSupportedException("Can't use IsDomainDifferent. Read Domain declaration for entity first");
            }
            if (!row.TryGetValue(Domain, out var domain))
            {
                throw new NotSupportedException("Can't use IsDomainDifferent. Read Domain declaration for property first");
            }
            return !entityDomain?.Equals(domain) ?? false;
        }) { ReadOrder = 1 };
}