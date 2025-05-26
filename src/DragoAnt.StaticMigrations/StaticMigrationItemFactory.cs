using DragoAnt.StaticMigrations.MigrationConditions;
using System.Collections.Immutable;

namespace DragoAnt.StaticMigrations;

public record StaticMigrationItemFactory<T, TContext>(string Name, Func<TContext, T> Factory,IImmutableSet<string> Tags, Func<StaticMigrationConditionOptions, bool>? Condition);