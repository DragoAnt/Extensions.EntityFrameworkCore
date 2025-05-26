using System.Diagnostics;
using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.Contacts.TriggerBased;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

[DebuggerDisplay("{Name}, Id = {Id}")]
[CreateAuditedOptions(HasValueGenerator = false)] //Disable it. We can't use value generator for dict entity
public class Role : Entity,
    ICreateAuditedEntityConvention,
    IUpdateAuditedEntityConvention,
    ISoftDeleteEntityConvention
{
    public string Name { get; private set; }
    public string Description { get; set; }

    public static Role Create(string id, string name, string desc = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }

        id = id.ToUpper();
        var idGuid = Guid.Parse(id);
        return new Role { Id = idGuid, Name = name, Description = desc };
    }
}