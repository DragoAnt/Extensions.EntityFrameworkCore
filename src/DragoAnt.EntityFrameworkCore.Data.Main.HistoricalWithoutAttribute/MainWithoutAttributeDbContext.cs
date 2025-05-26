using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public class MainTypeRegistrationDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected MainTypeRegistrationDbContext()
    {
    }

    /// <inheritdoc />
    public MainTypeRegistrationDbContext(DbContextOptions<MainTypeRegistrationDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CurrencyMap());
        modelBuilder.ApplyConfiguration(new VCurrencyMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new ContactMap());
        modelBuilder.ApplyConfiguration(new Contact2Map());
        modelBuilder.ApplyConfiguration(new AnimalMap());
        modelBuilder.ApplyConfiguration(new CatMap());
        modelBuilder.ApplyConfiguration(new ElefantMap());
    }
}