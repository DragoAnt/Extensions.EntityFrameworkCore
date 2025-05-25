using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Definitions;

public interface IEFEntityDefinition : IDefinition
{
    object? Extract(IEntityType type, EntityDefinitionRow row, DefinitionContext context);
}

public interface IEFEntityDefinition<T> : IEFEntityDefinition, IDefinition<T>
{
    /// <inheritdoc />
    object? IEFEntityDefinition.Extract(IEntityType type, EntityDefinitionRow row, DefinitionContext context)
    {
        return Extract(type, row, context);
    }

    new T? Extract(IEntityType type, EntityDefinitionRow row, DefinitionContext context);
}