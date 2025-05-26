using Microsoft.EntityFrameworkCore;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public interface IDbContextOptionsConfigurator
{
    void Configure(DbContextOptionsBuilder builder);
}