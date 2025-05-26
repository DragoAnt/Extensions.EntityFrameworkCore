using System;
using Stenn.EntityDefinition.Contracts;
using Stenn.EntityDefinition.Contracts.Definitions;

namespace Stenn.EntityDefinition.Flowchart.Types
{
    //TODO: Implement it for graph based on clr type system
    
    public sealed class FlowchartEntityOptions :FlowchartEntityOptionsBase, IFlowchartEntityOptions
    {
        /// <inheritdoc />
        DefinitionInfo<string> IFlowchartEntityOptions.Id => Id;

        /// <inheritdoc />
        DefinitionInfo<string> IFlowchartEntityOptions.Caption => Caption;

        /// <inheritdoc />
        DefinitionInfo<Type> IFlowchartEntityOptions.Type => Type;

        /// <inheritdoc />
        DefinitionInfo<Type> IFlowchartEntityOptions.BaseType => BaseType;

        /// <inheritdoc />
        DefinitionInfo<bool> IFlowchartEntityOptions.IsAbstract => IsAbstract;

        public Definition<string> Id { get; set; } = EFCommonDefinitions.Entities.Name;
        public Definition<string> Caption { get; set; } = EFCommonDefinitions.Entities.Name;
        public Definition<Type> Type { get; set; } = EFCommonDefinitions.Entities.EntityType;
        public Definition<Type> BaseType { get; set; } = EFCommonDefinitions.Entities.BaseEntityType;
        public Definition<bool> IsAbstract { get; set; } = EFCommonDefinitions.Entities.IsAbstract;
    }
}