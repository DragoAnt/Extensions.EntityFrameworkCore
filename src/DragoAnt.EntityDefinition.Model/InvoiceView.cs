using DragoAnt.EntityDefinition.Model.Definitions;

namespace DragoAnt.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public abstract class InvoiceView
{
    public int Id { get; set; }
    public Money Fee { get; set; }

}