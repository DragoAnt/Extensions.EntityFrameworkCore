using DragoAnt.EntityFrameworkCore.Data.Initial.DictEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.Data.Initial.Configurations;

public class CurrencyV1Map: IEntityTypeConfiguration<CurrencyV1>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CurrencyV1> builder)
    {
        builder.ToTable("Currency");
        builder.Property(x => x.Iso3LetterCode).IsUnicode(false).HasMaxLength(3).IsFixedLength().IsRequired();
        builder.HasKey(x => x.Iso3LetterCode);
            
        builder.Property(x => x.IsoNumericCode).IsRequired();
        builder.Property(x => x.DecimalDigits).IsRequired();
        builder.Property(x => x.Description).IsUnicode().HasMaxLength(150);

        builder.HasData(CurrencyDeclaration.GetActual());
    }
}