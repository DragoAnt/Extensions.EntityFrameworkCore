using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.EntityDefinition.Flowchart;

public interface IFlowchartElementOptionsBase<in TRow>
    where TRow : DefinitionRowBase
{
    IReadOnlyCollection<FlowchartGraphGroup> GraphGroups { get; }
    FlowchartGraphDirection GroupDirection { get; }

    public bool Filter(TRow row);
}