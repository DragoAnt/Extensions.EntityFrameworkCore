using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityDefinition.Model.Configurations;

public class InvoiceMap : IEntityTypeConfiguration<Invoice>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        //builder.ToTable("Invoices");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Fee,
            c =>
            {
                c.Property(x => x.Amount);
                c.OwnsOne(x => x.Currency, currency => { currency.Property(a => a.IsoNumericCode); });
            });

        builder.Property<int>("ShadowComputed").HasComputedColumnSql("Id + 1");
    }
}