using DragoAnt.EntityDefinition.Contracts;
using DragoAnt.EntityDefinition.EntityFrameworkCore.Definitions;
using DragoAnt.EntityDefinition.Flowchart;

namespace DragoAnt.EntityDefinition.EntityFrameworkCore.Flowchart;

public sealed class EFFlowchartPropertyOptions : FlowchartPropertyOptionsBase, IFlowchartPropertyOptions
{
    /// <inheritdoc />
    DefinitionInfo<object> IFlowchartPropertyOptions.PropertyKey => PropertyKey;

    /// <inheritdoc />
    DefinitionInfo<string> IFlowchartElementOptions<PropertyDefinitionRow>.Caption => Caption;
        
    /// <inheritdoc />
    DefinitionInfo<string> IFlowchartElementOptions<PropertyDefinitionRow>.Id => ItemId;

    /// <inheritdoc />
    DefinitionInfo<bool> IFlowchartPropertyOptions.IsNavigation => IsNavigation;

    /// <inheritdoc />
    DefinitionInfo<bool?> IFlowchartPropertyOptions.IsNavigationCollection => IsCollection;

    /// <inheritdoc />
    DefinitionInfo<bool?> IFlowchartPropertyOptions.IsOnDependent => IsOnDependent;

    /// <inheritdoc />
    DefinitionInfo<Type> IFlowchartPropertyOptions.TargetType => TargetType;

    /// <inheritdoc />
    DefinitionInfo<object> IFlowchartPropertyOptions.TargetId => TargetId;

    /// <inheritdoc />
    DefinitionInfo<string> IFlowchartPropertyOptions.RelationCaption => RelationCaption;

    /// <inheritdoc />
    DefinitionInfo<string> IFlowchartPropertyOptions.RelationTooltip => RelationTooltip;

    public EFPropertyDefinition<object> PropertyKey { get; set; } = EFCommonDefinitions.Properties.PropertyKey;
    public EFPropertyDefinition<string> ItemId { get; set; } = EFCommonDefinitions.Properties.Name;
    public EFPropertyDefinition<string> Caption { get; set; } = EFCommonDefinitions.Properties.Name;
    public EFPropertyDefinition<bool> IsNavigation { get; set; } = EFCommonDefinitions.Properties.IsNavigation;
    public EFPropertyDefinition<bool?> IsCollection { get; set; } = EFCommonDefinitions.Properties.Navigation.IsNavigationCollection;
    public EFPropertyDefinition<bool?> IsOnDependent { get; set; } = EFCommonDefinitions.Properties.Navigation.IsOnDependent;
    public EFPropertyDefinition<Type> TargetType { get; set; } = EFCommonDefinitions.Properties.Navigation.TargetEntityType;
    public EFPropertyDefinition<object> TargetId { get; set; } = EFCommonDefinitions.Properties.Navigation.TargetPropertyId;
    public EFPropertyDefinition<string> RelationCaption { get; set; } = EFCommonDefinitions.Properties.Navigation.LongRelationCaption;
    public EFPropertyDefinition<string> RelationTooltip { get; set; } = EFCommonDefinitions.Properties.Navigation.RelationTooltip;
}