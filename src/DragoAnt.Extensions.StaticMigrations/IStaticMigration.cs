namespace DragoAnt.Extensions.StaticMigrations;

public interface IStaticMigration
{
    byte[] GetHash();
}