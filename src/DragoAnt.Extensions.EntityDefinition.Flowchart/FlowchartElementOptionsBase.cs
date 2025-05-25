using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public abstract class FlowchartElementOptionsBase<TRow>:IFlowchartElementOptionsBase<TRow>
    where TRow : DefinitionRowBase
{
    private Func<TRow, bool> _filter = _ => true;
    private readonly List<FlowchartGraphGroup> _graphGroups = new();

    public FlowchartGraphDirection GroupDirection { get; set; } = FlowchartGraphDirection.TB;
        
    public void SetFilter(Func<TRow, bool>? filter)
    {
        _filter = filter ?? (_ => true);
    }

    public void AddGraphGroup(FlowchartGraphGroup group)
    {
        _graphGroups.Add(group);
    }

    IReadOnlyCollection<FlowchartGraphGroup> IFlowchartElementOptionsBase<TRow>.GraphGroups =>_graphGroups;

    bool IFlowchartElementOptionsBase<TRow>.Filter(TRow row)
    {
        return _filter(row);
    }
        
}