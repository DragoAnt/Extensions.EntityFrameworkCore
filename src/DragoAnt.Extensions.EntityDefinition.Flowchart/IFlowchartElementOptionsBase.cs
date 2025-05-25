using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public interface IFlowchartElementOptionsBase<in TRow>
    where TRow : DefinitionRowBase
{
    IReadOnlyCollection<FlowchartGraphGroup> GraphGroups { get; }
    FlowchartGraphDirection GroupDirection { get; }

    public bool Filter(TRow row);
}