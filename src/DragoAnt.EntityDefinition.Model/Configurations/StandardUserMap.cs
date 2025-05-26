using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityDefinition.Model.Configurations;

public class StandardUserMap: IEntityTypeConfiguration<StandardUser>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<StandardUser> builder)
    {
        builder.HasBaseType<User>();
    }
}