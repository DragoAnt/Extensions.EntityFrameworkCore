namespace DragoAnt.Extensions.StaticMigrations.MigrationConditions;

public interface IWithStaticMigrationActionTag
{
    public string[] Tags { get; }
}