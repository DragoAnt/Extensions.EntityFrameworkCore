using DragoAnt.EntityDefinition.Contracts;

namespace DragoAnt.EntityDefinition.Flowchart;

public abstract class FlowchartEntityOptionsBase : FlowchartElementOptionsBase<EntityDefinitionRow>
{
    public bool AddInheritRelations { get; set; } = true;
}