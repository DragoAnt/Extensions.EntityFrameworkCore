namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public abstract class AuditedEntity : Entity
{
    protected AuditedEntity()
    {
        Created = DateTime.UtcNow;
    }

    public DateTime Created { get; private set; }
}