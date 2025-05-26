using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

public sealed class HistoricalMigrationsOptions : IEquatable<HistoricalMigrationsOptions>
{
    private Type? _dbContextType;

    internal HistoricalMigrationsOptions()
    {
    }

    /// <summary>
    /// Use full migrations history and ignore initial migrations
    /// </summary>
    public bool MigrateFromFullHistory { get; set; }

    public Type? DbContextType
    {
        get => _dbContextType;
        set
        {
            if (value != null && !value.IsAssignableTo(typeof(DbContext)))
            {
                throw new ArgumentException("Must be inherited from DbContext", nameof(value));
            }
            _dbContextType = value;
        }
    }

    /// <inheritdoc />
    public bool Equals(HistoricalMigrationsOptions? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return MigrateFromFullHistory == other.MigrateFromFullHistory;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is HistoricalMigrationsOptions other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return MigrateFromFullHistory.GetHashCode();
    }
}