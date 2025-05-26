using DragoAnt.EntityDefinition.Contracts;

namespace DragoAnt.EntityDefinition.Flowchart;

public interface IFlowchartEntityOptions : IFlowchartElementOptions<EntityDefinitionRow>
{
    /// <summary>
    /// Add entities inheritance relations
    /// </summary>
    bool AddInheritRelations { get; }

    DefinitionInfo<Type> Type { get; }
    DefinitionInfo<Type> BaseType { get; }
    DefinitionInfo<bool> IsAbstract { get; }
}