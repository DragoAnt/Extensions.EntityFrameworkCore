namespace DragoAnt.StaticMigrations;

public interface IStaticMigration
{
    byte[] GetHash();
}