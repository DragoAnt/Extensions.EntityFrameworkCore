using DragoAnt.Extensions.StaticMigrations.MigrationConditions;
using System.Collections.Immutable;

namespace DragoAnt.Extensions.StaticMigrations;

public record StaticMigrationItemFactory<T, TContext>(string Name, Func<TContext, T> Factory,IImmutableSet<string> Tags, Func<StaticMigrationConditionOptions, bool>? Condition);