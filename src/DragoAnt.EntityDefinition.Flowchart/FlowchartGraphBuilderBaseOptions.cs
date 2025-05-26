using System.Drawing;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.EntityDefinition.Flowchart;

public abstract class FlowchartGraphBuilderBaseOptions<TEntityOptions, TPropertyOptions> : IFlowchartGraphBuilderOptions
    where TEntityOptions : class, IFlowchartEntityOptions, new()
    where TPropertyOptions : class, IFlowchartPropertyOptions, new()
{
    private Action<FlowchartStyleClass> _initAbstractEntityStyleClassAction = InitArstractEntityStyleClassDefault;
    private Action<FlowchartStyleClass> _initRelationNodeStyleClassActionAction = InitRelationNodeStyleClassDefault;
    private Func<FlowchartGraphItem, bool> _skipForCleaningFilter = _ => false;
        
    public bool DrawRelationAsNode { get; set; }
        
    /// <summary>
    /// Remove all nodes from graph without relations including groups
    /// </summary>
    public bool CleanNodesWithoutRelations { get; set; }
        
    /// <inheritdoc />
    bool IFlowchartGraphBuilderOptions.SkipFromCleaningFilter(FlowchartGraphItem cleaningItem)
    {
        return _skipForCleaningFilter(cleaningItem);
    }

    public void SetSkipFromCleaningFilter(Func<FlowchartGraphItem, bool> filter)
    {
        _skipForCleaningFilter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    public FlowchartGraphDirection Direction { get; set; } = FlowchartGraphDirection.LR;
        
    Action<FlowchartStyleClass> IFlowchartGraphBuilderOptions.InitAbstractEntityStyleClassAction => _initAbstractEntityStyleClassAction;

    public void InitArstractEntityStyleClass(Action<FlowchartStyleClass> init)
    {
        _initAbstractEntityStyleClassAction = init ?? throw new ArgumentNullException(nameof(init));
    }

    private static void InitArstractEntityStyleClassDefault(FlowchartStyleClass styleClass)
    {
        styleClass.SetFontWeight("bold");
        styleClass.SetFontStyle("italic");
        styleClass.SetColor(Color.Blue);
    }
        
    public void InitRelationNodeStyleClass(Action<FlowchartStyleClass> init)
    {
        _initRelationNodeStyleClassActionAction = init ?? throw new ArgumentNullException(nameof(init));
    }

    public Action<FlowchartStyleClass> InitRelationNodeStyleClassAction => _initRelationNodeStyleClassActionAction;

    private static void InitRelationNodeStyleClassDefault(FlowchartStyleClass styleClass)
    {
        //styleClass.SetModifier("font-style", "italic");
        styleClass.SetFill(Color.Azure);
        styleClass.SetModifier("font-size", "80%");
        styleClass.SetColor(Color.DimGray);
    }
        
    IFlowchartEntityOptions IFlowchartGraphBuilderOptions.Entity => Entity;
    public TEntityOptions Entity { get; } = new();

    IFlowchartPropertyOptions IFlowchartGraphBuilderOptions.Property => Property;
    public TPropertyOptions Property { get; } = new();
}