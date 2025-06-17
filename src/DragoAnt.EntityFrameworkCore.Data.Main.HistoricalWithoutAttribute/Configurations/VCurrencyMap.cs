using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.Configurations;

public class VCurrencyMap: IEntityTypeConfiguration<VCurrency>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VCurrency> builder)
    {
        RelationalEntityTypeBuilderExtensions.ToView((EntityTypeBuilder)builder, "vCurrency");
        builder.HasKey(x => x.Iso3LetterCode);
    }
}