using DragoAnt.Extensions.EntityDefinition.Contracts;

namespace DragoAnt.Extensions.EntityDefinition.Flowchart;

public interface IFlowchartElementOptions<in TRow> : IFlowchartElementOptionsBase<TRow>
    where TRow : DefinitionRowBase
{
    DefinitionInfo<string> Id { get; }
    DefinitionInfo<string> Caption { get; }
}