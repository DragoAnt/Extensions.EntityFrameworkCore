using Microsoft.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;

public interface IDbContextOptionsConfigurator
{
    void Configure(DbContextOptionsBuilder builder);
}