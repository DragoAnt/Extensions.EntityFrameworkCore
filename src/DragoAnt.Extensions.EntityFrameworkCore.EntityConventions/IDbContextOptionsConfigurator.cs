using Microsoft.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public interface IDbContextOptionsConfigurator
{
    void Configure(DbContextOptionsBuilder builder);
}