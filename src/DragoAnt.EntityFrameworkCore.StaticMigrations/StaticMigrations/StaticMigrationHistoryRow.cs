namespace DragoAnt.EntityFrameworkCore.StaticMigrations.StaticMigrations;

public class StaticMigrationHistoryRow
{
    public StaticMigrationHistoryRow(string name, byte[] hash, DateTime modified)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        }
        Name = name;
        Hash = hash;
        Modified = modified;
    }

    public string Name { get; }
    public byte[] Hash { get; }
    public DateTime Modified { get; set; }
}