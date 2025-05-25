using DragoAnt.Extensions.EntityDefinition.Model.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public abstract class InvoiceView
{
    public int Id { get; set; }
    public Money Fee { get; set; }

}