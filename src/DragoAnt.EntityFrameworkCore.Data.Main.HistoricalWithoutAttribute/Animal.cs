using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.Contacts.TriggerBased;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

[DiscriminatorOptions(MaxLength = 20, IsUnicode = false)]
public abstract class Animal : IWithDiscriminatorEntityConvention<string>,
    ICreateAuditedEntityConvention,
    IUpdateAuditedEntityConvention
{
    public int Id { get; set; }
}

public class Elefant : Animal
{
}

public class Cat : Animal
{
}