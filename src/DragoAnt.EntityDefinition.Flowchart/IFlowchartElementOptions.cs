using DragoAnt.EntityDefinition.Contracts;

namespace DragoAnt.EntityDefinition.Flowchart;

public interface IFlowchartElementOptions<in TRow> : IFlowchartElementOptionsBase<TRow>
    where TRow : DefinitionRowBase
{
    DefinitionInfo<string> Id { get; }
    DefinitionInfo<string> Caption { get; }
}