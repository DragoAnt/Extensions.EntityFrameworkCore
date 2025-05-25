using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityDefinition.Model.Configurations;

public class SuperUserMap: IEntityTypeConfiguration<SuperUser>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SuperUser> builder)
    {
        builder.HasBaseType<User>();
    }
}