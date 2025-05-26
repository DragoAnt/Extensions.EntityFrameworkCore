using DragoAnt.EntityDefinition.EntityFrameworkCore.Definitions;
using DragoAnt.EntityDefinition.Flowchart;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.EntityDefinition.EntityFrameworkCore.Flowchart;

public static class EFFlowchartGraphGroupingExtensions
{
    public static FlowchartGraphGroup ToFlowchartGraphGrouping<T>(this EFEntityDefinition<T> definition,
        Action<T?, FlowchartStyleClass>? fillStyle = null,
        Func<T?, string>? extractItemId = null,
        Func<T?, string?>? extractCaption = null, bool skipDuringClean = false)
    {
        return definition.Info.ToFlowchartGraphGroup(fillStyle,extractItemId, extractCaption, skipDuringClean);
    }

    public static FlowchartGraphGroup ToFlowchartGraphGrouping<T>(this EFPropertyDefinition<T> definition,
        Action<T?, FlowchartStyleClass>? fillStyle = null,
        Func<T?, string>? extractItemId = null,
        Func<T?, string?>? extractCaption = null, bool skipDuringClean = false)
    {
        return definition.Info.ToFlowchartGraphGroup(fillStyle,extractItemId, extractCaption, skipDuringClean);
    }
}