namespace DragoAnt.EntityFrameworkCore.Data.Main;

public abstract class AuditedEntity : Entity
{
    protected AuditedEntity()
    {
        Created = DateTime.UtcNow;
    }

    public DateTime Created { get; private set; }
}