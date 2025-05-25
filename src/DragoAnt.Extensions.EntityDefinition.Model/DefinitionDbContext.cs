using Microsoft.EntityFrameworkCore;
using DragoAnt.Extensions.EntityDefinition.Model.Configurations;

namespace DragoAnt.Extensions.EntityDefinition.Model;

public class DefinitionDbContext: DbContext
{
    /// <inheritdoc />
    protected DefinitionDbContext()
    {
    }

    /// <inheritdoc />
    public DefinitionDbContext(DbContextOptions<DefinitionDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new StandardUserMap());
        modelBuilder.ApplyConfiguration(new SuperUserMap());
            
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new UserRoleMap());
            
        modelBuilder.ApplyConfiguration(new InvoiceMap());
        modelBuilder.ApplyConfiguration(new InvoiceViewMap());
        modelBuilder.ApplyConfiguration(new InvoiceViewExtendedMap());

        base.OnModelCreating(modelBuilder);
    }
}