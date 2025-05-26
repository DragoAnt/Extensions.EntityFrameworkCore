namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

/// <summary>
/// Mark Entity Framework migration to show
/// that this migration has previous migrations splitted to another assembly
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class HistoricalMigrationAttribute : Attribute
{
    /// <inheritdoc />
    public HistoricalMigrationAttribute(Type dbContextAssemblyAnchorType)
    {
        DBContextAssemblyAnchorType = dbContextAssemblyAnchorType;
    }

    /// <summary>
    /// Special historical migration that replace all previous migrations.
    /// If any of the previous migrations did't applied yet, they will be applied and specified migration will be skipped.
    /// And row about this historical migration in history table will replace previous migrations rows 
    /// </summary>
    public bool Initial { get; set; }

    /// <summary>
    /// DbContext type of historical migrations anchor
    /// </summary>
    public Type DBContextAssemblyAnchorType { get; }
}