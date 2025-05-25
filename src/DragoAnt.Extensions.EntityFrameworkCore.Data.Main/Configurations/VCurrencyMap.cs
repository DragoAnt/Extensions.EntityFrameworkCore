using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Configurations;

public class VCurrencyMap: IEntityTypeConfiguration<VCurrency>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VCurrency> builder)
    {
        RelationalEntityTypeBuilderExtensions.ToView((EntityTypeBuilder)builder, "vCurrency");
        builder.HasKey(x => x.Iso3LetterCode);
    }
}