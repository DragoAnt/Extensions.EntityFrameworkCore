using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Initial.Configurations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;

public class InitialDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected InitialDbContext()
    {
    }

    /// <inheritdoc />
    public InitialDbContext(DbContextOptions<InitialDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CurrencyV1Map());
        modelBuilder.ApplyConfiguration(new VCurrencyMap());

        base.OnModelCreating(modelBuilder);
    }
}