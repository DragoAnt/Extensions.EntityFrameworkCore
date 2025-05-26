using System.Collections.Immutable;

namespace DragoAnt.StaticMigrations.MigrationConditions;

public interface IStaticMigrationConditionItem
{
    /// <summary>
    /// The migration was never applied to a db
    /// </summary>
    public bool IsNew { get; }
    public string Name { get; }
    public IImmutableSet<string> Tags { get; }
}