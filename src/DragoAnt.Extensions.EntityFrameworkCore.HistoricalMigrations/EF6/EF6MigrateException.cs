using System.Runtime.Serialization;

namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

[Serializable]
public class EF6MigrateException : Exception
{
    /// <inheritdoc />
    public EF6MigrateException()
    {
    }

    /// <inheritdoc />
    public EF6MigrateException(string? message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public EF6MigrateException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <inheritdoc />
    [Obsolete]
    protected EF6MigrateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}