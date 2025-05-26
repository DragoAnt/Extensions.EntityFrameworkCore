using DragoAnt.EntityDefinition.Contracts;

namespace DragoAnt.EntityDefinition;

public interface IDefinitionReader
{
    DefinitionMap Read();
}