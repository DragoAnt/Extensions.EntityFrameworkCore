using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public interface IFlowchartGraphBuilderOptions
{
    /// <summary>
    /// Grapth direction
    /// </summary>
    FlowchartGraphDirection Direction { get; }

    /// <summary>
    /// Use individual graph node for draw property's relation
    /// </summary>
    public bool DrawRelationAsNode { get; }

    /// <summary>
    /// Remove all nodes from graph without relations including groups
    /// </summary>
    bool CleanNodesWithoutRelations { get; }

    bool SkipFromCleaningFilter(FlowchartGraphItem cleaningItem);
        
    Action<FlowchartStyleClass> InitAbstractEntityStyleClassAction { get; }
    Action<FlowchartStyleClass> InitRelationNodeStyleClassAction { get; }

    IFlowchartEntityOptions Entity { get; }
    IFlowchartPropertyOptions Property { get; }
}