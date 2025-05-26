using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

/// <summary>
/// Db specific service for entity convention
/// </summary>
public interface IEntityConventionsProviderService
{
    void Configure(EntityTypeBuilder builder);
}