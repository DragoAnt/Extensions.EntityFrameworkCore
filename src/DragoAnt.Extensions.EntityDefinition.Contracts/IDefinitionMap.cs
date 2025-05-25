namespace DragoAnt.Extensions.EntityDefinition.Contracts;

public interface IDefinitionMap
{
    IReadOnlyCollection<DefinitionInfo> EntityDefinitions { get; }
    IReadOnlyCollection<DefinitionInfo> PropertyDefinitions { get; }
    IReadOnlyCollection<EntityDefinitionRow> Entities { get; }
}