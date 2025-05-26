using DragoAnt.EntityDefinition.Model.Definitions;

namespace DragoAnt.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public class InvoiceViewExtended: InvoiceView
{
    public string ExtraData { get; set; }

}