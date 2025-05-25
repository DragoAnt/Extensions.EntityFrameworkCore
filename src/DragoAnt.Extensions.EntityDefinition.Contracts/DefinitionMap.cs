namespace DragoAnt.Extensions.EntityDefinition.Contracts;

public sealed class DefinitionMap : IDefinitionMap
{
    /// <inheritdoc />
    IReadOnlyCollection<EntityDefinitionRow> IDefinitionMap.Entities => Entities;

    public IReadOnlyCollection<DefinitionInfo> EntityDefinitions { get; }

    public IReadOnlyCollection<DefinitionInfo> PropertyDefinitions { get; }

    internal List<EntityDefinitionRow> Entities { get; }

    public DefinitionMap(IReadOnlyCollection<DefinitionInfo> entityDefinitions,
        IReadOnlyCollection<DefinitionInfo> propertyDefinitions)
    {
        EntityDefinitions = entityDefinitions;
        PropertyDefinitions = propertyDefinitions;
        Entities = new List<EntityDefinitionRow>();
    }

    public EntityDefinitionRowBuilder Add(string name)
    {
        var entityDefinition = new EntityDefinitionRow(name, EntityDefinitions.Count);
        Entities.Add(entityDefinition);

        return new EntityDefinitionRowBuilder(this, entityDefinition);
    }
}