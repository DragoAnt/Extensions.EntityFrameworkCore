using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Configurations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalInitial;

public class HistoricalInitialMainDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected HistoricalInitialMainDbContext()
    {
    }

    /// <inheritdoc />
    public HistoricalInitialMainDbContext(DbContextOptions<HistoricalInitialMainDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CurrencyMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new ContactMap());
        modelBuilder.ApplyConfiguration(new Contact2Map());
        modelBuilder.ApplyConfiguration(new AnimalMap());
        modelBuilder.ApplyConfiguration(new CatMap());
        modelBuilder.ApplyConfiguration(new ElefantMap());
    }
}