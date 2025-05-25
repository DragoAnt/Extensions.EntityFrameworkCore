namespace DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;

public sealed class StaticMigrationsOptions
{
    /// <summary>
    /// Enable enums' tables migration
    /// </summary>
    public bool EnableEnumTables { get; set; } = true;
}