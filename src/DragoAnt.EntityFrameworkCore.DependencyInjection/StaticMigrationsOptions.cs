namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

public sealed class StaticMigrationsOptions
{
    /// <summary>
    /// Base directory location of static sql scripts in assembly with migrations.
    /// </summary>
    public string SqlScriptsPath { get; set; } = @"\Migrations.Static\Sql\";
    
    /// <summary>
    /// Enable enums' tables migration
    /// </summary>
    public bool EnableEnumTables { get; set; } = true;
}