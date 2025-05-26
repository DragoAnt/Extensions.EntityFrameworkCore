using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public interface IEntityConventionsService
{
    bool HasConventions { get; }
    void Configure(EntityTypeBuilder entityBuilder);
}