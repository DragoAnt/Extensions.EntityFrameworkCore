using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

internal sealed class CreateAuditedEntityValueGenerator : ValueGenerator
{
    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <inheritdoc />
    protected override object NextValue(EntityEntry entry)
    {
        return DateTime.Now;
    }
}