using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

namespace DragoAnt.EntityFrameworkCore.Data.Main.EF6Initial;

public class EF6InitialMainDbContext: Microsoft.EntityFrameworkCore.DbContext
{
    /// <inheritdoc />
    protected EF6InitialMainDbContext()
    {
    }

    /// <inheritdoc />
    public EF6InitialMainDbContext(DbContextOptions<EF6InitialMainDbContext> options)
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