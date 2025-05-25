using DragoAnt.Extensions.EntityDefinition.Model.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public class InvoiceViewExtended: InvoiceView
{
    public string ExtraData { get; set; }

}