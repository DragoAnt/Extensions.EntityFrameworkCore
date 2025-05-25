using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.StaticMigrations.DictEntities;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Configurations;

public class RoleMap: IEntityTypeConfiguration<Role>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired();
            
        builder.HasKey(x => x.Id);

        builder.HasData(RoleDeclaration.GetActual());
    }
}