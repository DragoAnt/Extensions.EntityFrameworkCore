using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public abstract class RelationalDbContextOptionsConfigurator : IDbContextOptionsConfigurator
{
    public abstract void Configure(DbContextOptionsBuilder builder);
}