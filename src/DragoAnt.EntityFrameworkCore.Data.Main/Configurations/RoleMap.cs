﻿using DragoAnt.EntityFrameworkCore.Data.Main.DictEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.Data.Main.Configurations;

public class RoleMap: IEntityTypeConfiguration<Role>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired();
            
        builder.HasKey(x => x.Id);

        builder.HasData(RoleDeclaration.GetActual());
    }
}