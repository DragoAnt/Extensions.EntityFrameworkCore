using DragoAnt.Extensions.EntityDefinition.Contracts;
using DragoAnt.Extensions.EntityDefinition.Model.Definitions;

namespace DragoAnt.Extensions.EntityDefinition.Model;

/// <summary>
/// User description test
/// </summary>
[DefinitionDomain(Domain.Security)]
[DefinitionRemark("Users' remark")]
public abstract class User
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// User's name
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Is user active otherwise user is archived
    /// </summary>
    [DefinitionDomain(Domain.Unknown)]
    public bool IsActive { get; set; }

    public virtual ICollection<UserRole> Roles { get; set; }
}