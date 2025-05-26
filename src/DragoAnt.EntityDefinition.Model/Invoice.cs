using DragoAnt.EntityDefinition.Model.Definitions;

namespace DragoAnt.EntityDefinition.Model;

[DefinitionDomain(Domain.Order)]
public class Invoice
{
    public int Id { get; set; }
    public Money Fee { get; set; }

}