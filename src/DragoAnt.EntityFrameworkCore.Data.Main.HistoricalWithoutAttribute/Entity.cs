namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public abstract class Entity:Entity<Guid>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
    
public abstract class Entity<TId>
{
    public TId Id { get; protected set; }
}