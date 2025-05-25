using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityDefinition.Model.Configurations;

public class UserMap: IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<StandardUser>(nameof(StandardUser))
            .HasValue<SuperUser>(nameof(SuperUser));
                
            
        builder.Property(x => x.Name).IsUnicode().HasMaxLength(250).IsRequired().HasColumnName("User_Name");
    }
}