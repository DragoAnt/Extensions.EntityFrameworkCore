using DragoAnt.Extensions.EntityDefinition.Contracts;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public abstract class FlowchartPropertyOptionsBase : FlowchartElementOptionsBase<PropertyDefinitionRow>
{
    /// <summary>
    /// Draw property as individual node
    /// </summary>
    public bool DrawAsNode { get; set; }
}