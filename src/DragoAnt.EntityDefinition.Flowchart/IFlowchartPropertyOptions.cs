using DragoAnt.EntityDefinition.Contracts;

namespace DragoAnt.EntityDefinition.Flowchart;

public interface IFlowchartPropertyOptions : IFlowchartElementOptions<PropertyDefinitionRow>
{
    /// <summary>
    /// Use individual graph node for draw property
    /// </summary>
    /// <returns></returns>
    bool DrawAsNode { get; }

    DefinitionInfo<object> PropertyKey { get; }
    DefinitionInfo<bool> IsNavigation { get; }
    DefinitionInfo<bool?> IsNavigationCollection { get; }
    DefinitionInfo<bool?> IsOnDependent { get; }
    DefinitionInfo<Type> TargetType { get; }
    DefinitionInfo<object> TargetId { get; }
    DefinitionInfo<string> RelationCaption { get; }
    DefinitionInfo<string> RelationTooltip { get; }
}