using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public interface IEntityConventionsService
{
    bool HasConventions { get; }
    void Configure(EntityTypeBuilder entityBuilder);
}