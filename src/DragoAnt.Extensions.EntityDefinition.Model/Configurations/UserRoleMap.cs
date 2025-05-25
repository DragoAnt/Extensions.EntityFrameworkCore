using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityDefinition.Model.Configurations;

public class UserRoleMap: IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(nameof(UserRole.UserId), nameof(UserRole.RoleId));
        builder.HasOne(x => x.User).WithMany(x => x.Roles);
        builder.HasOne(x => x.Role).WithMany(x => x.Users);
#pragma warning disable CS0612
        builder.Ignore(x => x.Obsolete);
#pragma warning restore CS0612
    }
}