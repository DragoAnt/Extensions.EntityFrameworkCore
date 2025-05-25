using DragoAnt.Extensions.EntityDefinition.Model.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public class Invoice
{
    public int Id { get; set; }
    public Money Fee { get; set; }

}