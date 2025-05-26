using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

internal sealed class SourceSystemIdValueGenerator : ValueGenerator
{
    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;
    /// <inheritdoc />
    protected override object NextValue(EntityEntry entry)
    {
        return ((IWithSourceSystemIdEntityConvention)entry.Entity).GenerateSourceSystemId();
    }
}