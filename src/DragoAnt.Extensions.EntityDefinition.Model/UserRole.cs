using DragoAnt.Extensions.EntityDefinition.Model.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model;

public class UserRole
{
    [DefinitionDomain(Domain.Security)]
    public int UserId { get; set; }
        
    [DefinitionDomain(Domain.Security)]
    public int RoleId { get; set; }
        
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
        
    [Obsolete]
    public int Obsolete { get; set; }
}