using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalInitial;

public class HistoricalInitialMainDbContext : Microsoft.EntityFrameworkCore.DbContext
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
#if NET9_0
        optionsBuilder.ConfigureWarnings(builder =>
            builder.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
#endif
    }
}