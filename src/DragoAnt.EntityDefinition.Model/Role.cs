using DragoAnt.EntityDefinition.Model.Definitions;

namespace DragoAnt.EntityDefinition.Model;

[DefinitionDomain(Domain.Security)]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
        
    public virtual ICollection<UserRole> Users { get; set; }
}