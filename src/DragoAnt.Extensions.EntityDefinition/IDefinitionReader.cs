using DragoAnt.Extensions.EntityDefinition.Contracts;

namespace DragoAnt.Extensions.EntityDefinition;

public interface IDefinitionReader
{
    DefinitionMap Read();
}