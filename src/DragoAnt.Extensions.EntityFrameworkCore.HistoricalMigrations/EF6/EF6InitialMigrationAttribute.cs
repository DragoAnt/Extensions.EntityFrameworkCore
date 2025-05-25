namespace DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

/// <summary>
///  Initial migration that moved from Entity Framework 6 to Entity Framework Core 
/// </summary>
public sealed class EF6InitialMigrationAttribute : Attribute
{
    private readonly Type _ef6MigrationManagerType;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="ef6MigrationManagerType">Migration manager. Must be inherited from <see cref="IEF6MigrationManager"/> and has parameterless constructor</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public EF6InitialMigrationAttribute(Type ef6MigrationManagerType)
    {
        if (ef6MigrationManagerType == null)
        {
            throw new ArgumentNullException(nameof(ef6MigrationManagerType));
        }
        if (!ef6MigrationManagerType.IsAssignableTo(typeof(IEF6MigrationManager)) &&
            ef6MigrationManagerType.GetConstructor(Type.EmptyTypes) == null)
        {
            throw new ArgumentException($"Type must be inherited from '{nameof(IEF6MigrationManager)}' and has parameterless constructor");
        }
        _ef6MigrationManagerType = ef6MigrationManagerType;
    }

    public IEF6MigrationManager GetManager()
    {
        return (IEF6MigrationManager?)Activator.CreateInstance(_ef6MigrationManagerType) ??
               throw new Exception($"Can't create instance of {_ef6MigrationManagerType.Name}");
    }
}