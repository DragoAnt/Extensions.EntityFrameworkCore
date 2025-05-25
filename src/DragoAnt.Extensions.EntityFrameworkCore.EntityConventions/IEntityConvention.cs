using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public interface IEntityConvention
{
    bool Allowed(IReadOnlyTypeBase entity);
    void Configure(EntityTypeBuilder builder);
}