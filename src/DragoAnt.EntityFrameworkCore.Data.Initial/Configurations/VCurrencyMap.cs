using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.Data.Initial.Configurations;

public class VCurrencyMap: IEntityTypeConfiguration<VCurrency>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VCurrency> builder)
    {
        builder.ToView("vCurrency");
        builder.HasKey(x => x.Iso3LetterCode);
    }
}