using Microsoft.EntityFrameworkCore.Metadata;
using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.Contracts.Definitions;

namespace DragoAnt.EntityDefinition.EntityFrameworkCore.Definitions;

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