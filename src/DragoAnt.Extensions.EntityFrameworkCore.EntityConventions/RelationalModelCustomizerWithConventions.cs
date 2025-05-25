using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public class RelationalModelCustomizerWithConventions : RelationalModelCustomizer
{
    /// <inheritdoc />
    public RelationalModelCustomizerWithConventions(ModelCustomizerDependencies dependencies)
        : base(dependencies)
    {
    }

    /// <inheritdoc />
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);
        ModelCustomizerWithConventions.DoCustomize(modelBuilder, context);
    }
}