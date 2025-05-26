namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

[AttributeUsage(AttributeTargets.Class)]
public sealed class InitialMigrationAttribute : Attribute
{
    /// <inheritdoc />
    public InitialMigrationAttribute(string[] removeMigrationRowIds)
    {
        RemoveMigrationRowIds = removeMigrationRowIds;
    }

    public string[] RemoveMigrationRowIds { get; }

}