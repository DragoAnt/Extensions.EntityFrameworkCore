using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.StaticMigrations;

public interface IDbContextOptionsConfigurator
{
    void Configure(DbContextOptionsBuilder builder);
}