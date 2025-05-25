using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Flowchart;

namespace DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Flowchart;

public sealed class EFFlowchartGraphBuilderOptions : 
    FlowchartGraphBuilderBaseOptions<EFFlowchartEntityOptions, EFFlowchartPropertyOptions>
{
    private Action<IEntityFrameworkCoreDefinitionOptions>? _initReaderAction;

    /// <summary>
    /// Initialize <see cref="IDefinitionMap"/> reader options. eg. for read custom definitions
    /// </summary>
    /// <param name="init"></param>
    public void InitReaderOptions(Action<IEntityFrameworkCoreDefinitionOptions> init)
    {
        _initReaderAction = init;
    }

    internal void InitReaderOptions(IEntityFrameworkCoreDefinitionOptions options)
    {
        _initReaderAction?.Invoke(options);
    }
}