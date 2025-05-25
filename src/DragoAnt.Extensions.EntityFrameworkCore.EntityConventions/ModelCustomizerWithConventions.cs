using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public class ModelCustomizerWithConventions : ModelCustomizer
{
    public static void DoCustomize(ModelBuilder modelBuilder, DbContext context)
    {
        var serviceProvider = context.GetInfrastructure();

        var entityConventionsProviderService = serviceProvider.GetRequiredService<IEntityConventionsProviderService>();
        var entityConventionsService = serviceProvider.GetRequiredService<IEntityConventionsService>();

        //NOTE: Convensions applying to root classes only
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => e.BaseType is null))
        {
#pragma warning disable EF1001
            var entityBuilder = new EntityTypeBuilder(entityType);
#pragma warning restore EF1001
            entityConventionsService.Configure(entityBuilder);
            entityConventionsProviderService.Configure(entityBuilder);
        }
    }

    /// <inheritdoc />
    public ModelCustomizerWithConventions(ModelCustomizerDependencies dependencies)
        : base(dependencies)
    {
    }

    /// <inheritdoc />
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);
        DoCustomize(modelBuilder, context);
    }
}