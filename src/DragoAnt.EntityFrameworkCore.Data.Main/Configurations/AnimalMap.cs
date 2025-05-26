using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.EntityFrameworkCore.EntityConventions;

namespace DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

public class AnimalMap: IEntityTypeConfiguration<Animal>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasConventionDiscriminator<Animal, string>()
            .HasValue<Cat>(nameof(Cat))
            .HasValue<Elefant>(nameof(Elefant));
    }
}
    
public class CatMap: IEntityTypeConfiguration<Cat>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        builder.HasBaseType<Animal>();
    }
}
    
public class ElefantMap: IEntityTypeConfiguration<Elefant>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Elefant> builder)
    {
        builder.HasBaseType<Animal>();
    }
}