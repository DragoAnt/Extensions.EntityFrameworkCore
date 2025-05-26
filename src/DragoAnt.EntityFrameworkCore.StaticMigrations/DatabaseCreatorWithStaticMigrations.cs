using Microsoft.EntityFrameworkCore.Storage;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public class DatabaseCreatorWithStaticMigrations : IDatabaseCreator
{
    private readonly IDatabaseCreator _databaseCreator;
    private readonly IStaticMigrationsService _staticMigrationsService;

    public DatabaseCreatorWithStaticMigrations(IDatabaseCreator databaseCreator, IStaticMigrationsService staticMigrationsService)
    {
        _databaseCreator = databaseCreator;
        _staticMigrationsService = staticMigrationsService;
    }

    /// <inheritdoc />
    public bool EnsureDeleted()
    {
        return _databaseCreator.EnsureDeleted();
    }

    /// <inheritdoc />
    public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.EnsureDeletedAsync(cancellationToken);
    }

    /// <inheritdoc />
    public bool EnsureCreated()
    {
        var result = _databaseCreator.EnsureCreated();
            
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        var result = await _databaseCreator.EnsureCreatedAsync(cancellationToken);
        return result;
    }

    /// <inheritdoc />
    public bool CanConnect()
    {
        return _databaseCreator.CanConnect();
    }

    /// <inheritdoc />
    public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        return _databaseCreator.CanConnectAsync(cancellationToken);
    }
}