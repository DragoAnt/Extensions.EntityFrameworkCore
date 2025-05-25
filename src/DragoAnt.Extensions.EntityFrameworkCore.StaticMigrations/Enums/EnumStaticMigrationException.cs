namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

public class EnumStaticMigrationException : StaticMigrationException
{
    /// <inheritdoc />
    public EnumStaticMigrationException(string? message = null)
        : base(message)
    {
    }

    /// <inheritdoc />
    public EnumStaticMigrationException(string? message, Exception? innerException) 
        : base(message, innerException)
    {
    }
}