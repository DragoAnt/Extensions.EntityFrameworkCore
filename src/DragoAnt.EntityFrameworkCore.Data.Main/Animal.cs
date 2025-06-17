using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.TriggerBased.Contacts;

namespace DragoAnt.EntityFrameworkCore.Data.Main;

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