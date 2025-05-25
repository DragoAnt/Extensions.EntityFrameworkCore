using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

public sealed class EFEntityDefinition<T> : Definition<T>, IEFEntityDefinition<T>
{
    private readonly Func<IEntityType, EntityDefinitionRow, DefinitionContext, T?> _extract;

    public static implicit operator EFEntityDefinition<T>(MemberInfoDefinition<T> d) => d.ToEntity();
        
    /// <inheritdoc />
    public EFEntityDefinition(DefinitionInfo<T> info, Func<IEntityType, EntityDefinitionRow, DefinitionContext, T?> extract)
        : base(info)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    /// <inheritdoc />
    public EFEntityDefinition(string name, Func<IEntityType, EntityDefinitionRow, DefinitionContext, T?> extract,
        Func<T, string>? convertToString = null)
        : base(name, convertToString)
    {
        _extract = extract ?? throw new ArgumentNullException(nameof(extract));
    }

    public T? Extract(IEntityType type, EntityDefinitionRow row, DefinitionContext context)
    {
        return _extract(type, row, context);
    }
}