using DragoAnt.StaticMigrations.MigrationConditions;
using System.Reflection;

namespace DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection;

/// <summary>
///  Standard extensions for static sql migrations based on guidlines that sql migrations stored in folder '<see cref="StaticMigrationBuilder.SqlScriptsPath"/>'  
/// </summary>
public static class CommonStaticMigrationsExtensions
{
    /// <summary>
    /// Gets standard path to resource file '<see cref="StaticMigrationBuilder.SqlScriptsPath"/>{filePath}{extension}' 
    /// </summary>
    /// <param name="migrations">Static migrations' builder</param>
    /// <param name="filePath">File name or path with name without extension </param>
    /// <param name="extension">Resource extension</param>
    /// <returns></returns>
    public static string GetFilePath(this StaticMigrationBuilder migrations, string filePath, string extension = ".sql")
        => Path.Combine(migrations.SqlScriptsPath, $"{filePath}{extension}");

    /// <summary>
    ///  Gets standard path to resource Apply file '<see cref="StaticMigrationBuilder.SqlScriptsPath"/>{filePath}.Apply.sql'
    /// </summary>
    /// <param name="migrations">Static migrations' builder</param>
    /// <param name="mainFileName">File name or path with name without extension</param>
    /// <returns></returns>
    public static string GetApplyFilePath(this StaticMigrationBuilder migrations, string mainFileName)
        => migrations.GetFilePath($"{mainFileName}.Apply");

    /// <summary>
    ///  Gets standard path to resource Revert file '<see cref="StaticMigrationBuilder.SqlScriptsPath"/>{filePath}.Revert.sql'
    /// </summary>
    /// <param name="migrations">Static migrations' builder</param>
    /// <param name="mainFileName">File name or path with name without extension</param>
    /// <returns></returns>
    public static string GetRevertFilePath(this StaticMigrationBuilder migrations, string mainFileName)
        => migrations.GetFilePath($"{mainFileName}.Revert");

    /// <summary>
    /// Adds sql res Initial static migration. Initial static migrations will be run before all other migrations(Static or classic EF)
    /// </summary>
    /// <param name="migrations">Static migrations' builder</param>
    /// <param name="mainFileName">File name or path with name without extension</param>
    /// <param name="suppressTransaction">Suppress transaction during run migration or not</param>
    /// <param name="assembly">Assembly with embedded resources</param>
    public static void AddInitialSqlResFile(
        this StaticMigrationBuilder migrations,
        string mainFileName,
        bool suppressTransaction = false,
        Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        migrations.AddInitResSql(mainFileName, migrations.GetFilePath(mainFileName), assembly, suppressTransaction: suppressTransaction);
    }

    /// <summary>
    /// Adds sql res static migration
    /// </summary>
    /// <param name="migrations">Static migrations' builder</param>
    /// <param name="mainFileName">File name or path with name without extension</param>
    /// <param name="type">Type of added resources</param>
    /// <param name="assembly">Assembly with embedded resources</param>
    /// <param name="condition">Migration will be applied if condition is true or null</param>
    public static void AddSqlResFile(
        this StaticMigrationBuilder migrations,
        string mainFileName,
        ResSqlFile type = ResSqlFile.All,
        Assembly? assembly = null,
        Func<StaticMigrationConditionOptions, bool>? condition = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        migrations.AddResSql(mainFileName,
            type.HasFlag(ResSqlFile.Apply) ? migrations.GetApplyFilePath(mainFileName) : null,
            type.HasFlag(ResSqlFile.Revert) ? migrations.GetRevertFilePath(mainFileName) : null,
            assembly,
            condition);
    }
}