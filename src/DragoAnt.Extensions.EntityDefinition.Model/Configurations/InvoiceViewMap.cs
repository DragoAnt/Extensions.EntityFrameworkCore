using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityDefinition.Model.Configurations;

public class InvoiceViewMap : IEntityTypeConfiguration<InvoiceView>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<InvoiceView> builder)
    {
        builder.ToView("vInvoices");

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<InvoiceViewExtended>("Extended");
            
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Fee,
            c =>
            {
                c.Property(x => x.Amount);
                c.OwnsOne(x => x.Currency, currency => { currency.Property(a => a.IsoNumericCode); });
            });
    }
}