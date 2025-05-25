using DragoAnt.Extensions.EntityDefinition.Contracts;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public abstract class FlowchartEntityOptionsBase : FlowchartElementOptionsBase<EntityDefinitionRow>
{
    public bool AddInheritRelations { get; set; } = true;
}