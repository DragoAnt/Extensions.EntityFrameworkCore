namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public class StaticMigrationException : Exception
{
    /// <inheritdoc />
    public StaticMigrationException(string? message = null)
        : base(message)
    {
    }

    /// <inheritdoc />
    public StaticMigrationException(string? message, Exception? innerException) 
        : base(message, innerException)
    {
    }
}