using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Configurations;

public class ContactMap: IEntityTypeConfiguration<Contact>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.TypeName).IsRequired()
            .HasMaxLength(100)
            .HasConversion(x => Enum.GetName(x), x => Enum.Parse<ContactStrType>(x));
            
        builder.Property(x => x.TypeName2).IsRequired()
            .HasMaxLength(100)
            .HasConversion<string>();
    }
}