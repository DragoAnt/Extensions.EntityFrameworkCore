using System;
using Stenn.EntityDefinition.Contracts;
using Stenn.EntityDefinition.Contracts.Definitions;

namespace Stenn.EntityDefinition.Flowchart.Types
{
    //TODO: Implement it for graph based on clr type system
    
    public sealed class FlowchartPropertyOptions : FlowchartPropertyOptionsBase, IFlowchartPropertyOptions
    {
        /// <inheritdoc />
        DefinitionInfo<object> IFlowchartPropertyOptions.Id => Id;

        /// <inheritdoc />
        DefinitionInfo<string> IFlowchartPropertyOptions.ItemId => ItemId;

        /// <inheritdoc />
        DefinitionInfo<string> IFlowchartPropertyOptions.Caption => Caption;

        /// <inheritdoc />
        DefinitionInfo<bool> IFlowchartPropertyOptions.IsNavigation => IsNavigation;

        /// <inheritdoc />
        DefinitionInfo<bool?> IFlowchartPropertyOptions.IsNavigationCollection => IsNavigationCollection;

        /// <inheritdoc />
        DefinitionInfo<bool?> IFlowchartPropertyOptions.IsOnDependent => IsOnDependent;

        /// <inheritdoc />
        DefinitionInfo<Type> IFlowchartPropertyOptions.TargetType => TargetType;

        /// <inheritdoc />
        DefinitionInfo<object> IFlowchartPropertyOptions.TargetId => TargetId;

        /// <inheritdoc />
        DefinitionInfo<string?> IFlowchartPropertyOptions.RelationCaption => RelationCaption;

        public Definition<object> Id { get; set; } = CommonDefinitions.PropertyInfo;
        public Definition<string> ItemId { get; set; } = CommonDefinitions.Name;
        public Definition<string> Caption { get; set; } = CommonDefinitions.Name;
        public Definition<bool> IsNavigation { get; set; } = CommonDefinitions.Navigation.IsNavigation;
        public Definition<bool?> IsNavigationCollection { get; set; } = CommonDefinitions.IsCollection;
        public Definition<bool?> IsOnDependent { get; set; } = CommonDefinitions.Navigation.IsOnDependent;
        public Definition<Type> TargetType { get; set; } = CommonDefinitions.Navigation.TargetEntityType;
        public Definition<object> TargetId { get; set; } = CommonDefinitions.Properties.Navigation.TargetPropertyId;
        public Definition<string?> RelationCaption { get; set; } = CommonDefinitions.Properties.Navigation.RelationCaption;
    }
}