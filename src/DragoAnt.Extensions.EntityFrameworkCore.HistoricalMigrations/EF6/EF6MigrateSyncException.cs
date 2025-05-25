using System.Runtime.Serialization;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

[Serializable]
public sealed class EF6MigrateSyncException : EF6MigrateException
{
    /// <inheritdoc />
    public EF6MigrateSyncException(string[] missed, string[] extra)
        : base("EFCore migrations and EF6 database are unsynced. " +
               $"Missed migrations in db:[{missed.Length}]{{ {string.Join(", ", missed)} }}. " +
               $"Extra migrations in db:[{extra.Length}]{{ {string.Join(", ", extra)} }}.")
    {
        Missed = missed;
        Extra = extra;
    }

    /// <inheritdoc />
    [Obsolete]
    public EF6MigrateSyncException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Missed = (string[]?)info.GetValue(nameof(Missed), typeof(string[]));
        Extra = (string[]?)info.GetValue(nameof(Extra), typeof(string[]));
    }

    /// <summary>
    /// Migrations presented in <see cref="IEF6MigrationManager.MigrationIds"/>, but doesn't present in db.
    /// You need migrate db to latest migration in manager by EF6 assembly or remove them from <see cref="IEF6MigrationManager.MigrationIds"/>
    /// if these migrations unnessesary   
    /// </summary>
    public string[]? Missed { get; }

    /// <summary>
    /// Migrations presented in database, but doesn't present <see cref="IEF6MigrationManager.MigrationIds"/>.
    /// You need to make next steps:
    /// 1. Check EFCore model and database via Compare tool.
    /// 2. Sync changes and regenerate Initial migration.
    /// 3. Add extra migrations to <see cref="IEF6MigrationManager.MigrationIds"/>
    /// </summary>
    public string[]? Extra { get; }

    /// <summary>
    /// Serialize object
    /// </summary>
    /// <param name="info">Serialization info</param>
    /// <param name="context">Streaming context</param>
#if NET8_0_OR_GREATER
    [Obsolete]
#endif
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Missed), Missed);
        info.AddValue(nameof(Extra), Extra);
    }
}