using System.Collections.Immutable;

namespace DragoAnt.Extensions.StaticMigrations.MigrationConditions;

public class StaticMigrationConditionOptions
{
    public StaticMigrationConditionOptions(IImmutableList<IStaticMigrationConditionItem> changedMigrations, IImmutableSet<string> tags)
    {
        ChangedMigrations = changedMigrations ?? throw new ArgumentNullException(nameof(changedMigrations));
        ForcedRunActionTags = tags ?? throw new ArgumentNullException(nameof(tags));
    }

    /// <summary>
    /// Static actions' tags forced to run
    /// </summary>
    public IImmutableSet<string> ForcedRunActionTags { get; }
        
    /// <summary>
    /// Changed migrations from previous run
    /// </summary>
    public IImmutableList<IStaticMigrationConditionItem> ChangedMigrations { get; }
}