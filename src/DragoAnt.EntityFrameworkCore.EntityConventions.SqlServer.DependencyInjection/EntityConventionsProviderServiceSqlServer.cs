using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;

public class EntityConventionsProviderServiceSqlServer : IEntityConventionsProviderService
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder builder)
    {
        foreach (var property in builder.Metadata.GetProperties())
        {
            var propBuilder = builder.Property(property.Name);
                
            if (property.FindAnnotation(ConventionsAnnotationNames.SqlDefault_CurrentDateTime) is { })
            {
                propBuilder.HasDefaultValueSql("getdate()");
                property.RemoveAnnotation(ConventionsAnnotationNames.SqlDefault_CurrentDateTime);
            }
            if (property.FindAnnotation(ConventionsAnnotationNames.ColumnTriggerUpdate_SqlDefault) is { })
            {
                var sqlDefault = property.GetAnnotation(RelationalAnnotationNames.DefaultValueSql);
                propBuilder.HasAnnotation(ConventionsAnnotationNames.ColumnTriggerUpdate, sqlDefault.Value);
                property.RemoveAnnotation(ConventionsAnnotationNames.ColumnTriggerUpdate_SqlDefault);
            }
        }
    }
}