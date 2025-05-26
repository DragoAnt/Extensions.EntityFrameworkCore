using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

namespace DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

public class VCurrencyMap: IEntityTypeConfiguration<VCurrency>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VCurrency> builder)
    {
        RelationalEntityTypeBuilderExtensions.ToView((EntityTypeBuilder)builder, "vCurrency");
        builder.HasKey(x => x.Iso3LetterCode);
    }
}