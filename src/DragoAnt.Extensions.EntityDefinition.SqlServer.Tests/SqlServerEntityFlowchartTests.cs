using System.Drawing;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore.Flowchart;
using DragoAnt.Extensions.EntityDefinition.Flowchart;
using DragoAnt.Extensions.EntityDefinition.Model.Definitions;
using DragoAnt.Shared.Mermaid;
using DragoAnt.Shared.Mermaid.Flowchart;

// ReSharper disable UnusedVariable

namespace DragoAnt.Extensions.EntityDefinition.SqlServer.Tests;

public class SqlServerEntityFlowchartTests : SqlServerEntityTestsBase
{
    [Fact]
    public void TestFlowchart()
    {
        var options = GetFlowchartGraphBuilderOptions();

        var graphBuilder = new EFFlowchartGraphBuilder(options);
        var outputEditor = graphBuilder.Build(DbContext).ToString(MermaidPrintConfig.Normal);
        var outputHtml = graphBuilder.Build(DbContext).ToString(MermaidPrintConfig.ForHtml);
    }

    private static EFFlowchartGraphBuilderOptions GetFlowchartGraphBuilderOptions()
    {
        var options = new EFFlowchartGraphBuilderOptions();
        options.InitReaderOptions(opt => { opt.AddEntityColumn(CustomDefinitions.Domain); });

        options.Property.DrawAsNode = false;
        options.DrawRelationAsNode = false;

        options.Entity.AddInheritRelations = false;
        options.CleanNodesWithoutRelations = true;

        //Uncomment for short ralation caption
        //options.Property.RelationCaption = EFCommonDefinitions.Properties.Navigation.ShortRelationCaption;

        //Uncomment for cross domain relations only
        //options.Property.SetFilter(propertyRow => propertyRow.GetValueOrDefault(CustomDefinitions.IsDomainDifferent.Info));

        options.Entity.AddGraphGroup(CustomDefinitions.Domain.ToFlowchartGraphGroup((domain, styleClass) =>
            {
                var color = domain switch
                {
                    Domain.Unknown => Color.LightGray,
                    Domain.Security => Color.PaleVioletRed,
                    Domain.Order => Color.Aquamarine,
                    _ => throw new ArgumentOutOfRangeException(nameof(domain), domain, null)
                };

                styleClass
                    .SetFill(color)
                    .SetStrokeWidth("2px")
                    .SetStrokeDashArray("2 2");
            },
            extractCaption: domain => $"Domain:{domain}",
            skipDuringClean: true)
        );

        return options;
    }
}