using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DragoAnt.Extensions.EntityConventions.Contacts;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public static class ModelBuilderConventionsExtensions
{
    public static void AddCommonConventions(this IEntityConventionsBuilder builder)
    {
        builder
            .AddCreateAudited()
            .AddConcurrentAudited()
            .AddWithDiscriminator()
            .AddWithSourceSystemId();
    }

    public static IEntityConventionsBuilder AddCreateAudited(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConventionProperty<ICreateAuditedEntityConvention>(x => x.Created,
            (e, _, p) =>
            {
                var options = e.Metadata.ClrType.GetCustomAttribute<CreateAuditedOptions>() ?? builder.DefaultOptions.CreateAudited;

                p.IsRequired()
                    .HasAnnotation(ConventionsAnnotationNames.SqlDefault_CurrentDateTime, true)
                    .ValueGeneratedOnAdd()
                    .HasComment("Row creation datetime. Configured by convention 'ICreateAuditedEntity'");

                if (options.HasValueGenerator)
                {
                    p.HasValueGenerator(options.Generator ?? typeof(CreateAuditedEntityValueGenerator));
                }

                FillPropertyOptions(e, p, options);
            });
        return builder;
    }

    public static IEntityConventionsBuilder AddConcurrentAudited(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConventionProperty<IConcurrentAuditedEntityConvention>(x => x.RowVersion,
            (_, _, p) => p.IsRequired()
                .IsRowVersion()
                .HasComment("Concurrent token(row version). Configured by convention 'IConcurrentAuditedEntity'"));

        return builder;
    }

    public static IEntityConventionsBuilder AddWithSourceSystemId(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConventionProperty<IWithSourceSystemIdEntityConvention>(x => x.SourceSystemId, (e, _, p) =>
        {
            var options = e.Metadata.ClrType.GetCustomAttribute<SourceSystemIdOptions>() ?? builder.DefaultOptions.SourceSystemId;

            p.IsRequired()
                .HasComment(
                    "Source system id. Row id for cross services' communication. Configured by convention 'IEntityWithSourceSystemId'");

            if (options.HasValueGenerator)
            {
                p.HasValueGenerator(options.Generator ?? typeof(SourceSystemIdValueGenerator));
            }

            FillStringPropertyOptions(e, p, options);
        });

        return builder;
    }

    public static IEntityConventionsBuilder AddWithDiscriminator(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConvention<IWithDiscriminatorEntityConvention>(e =>
        {
            var entityType = e.Metadata.ClrType;
            var options = entityType.GetCustomAttribute<DiscriminatorOptions>() ?? builder.DefaultOptions.Discriminator;

            Type propertyType;
            if (entityType.IsAssignableTo(typeof(IWithDiscriminatorEntityConvention<string>)))
            {
                propertyType = typeof(string);
            }
            else
            {
                var interfaceType = entityType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IWithDiscriminatorEntityConvention<>));
                if (interfaceType == null)
                {
                    throw new EntityConventionsException($"Entity '{entityType.Name}' must inherit from 'IEntityWithDiscriminator<>' interface");
                }
                propertyType = interfaceType.GetGenericArguments()[0];
            }
            var p = e.Property(propertyType, nameof(IWithDiscriminatorEntityConvention<string>.Discriminator));

            p.IsRequired()
                .HasComment("Discriminator. Configured by convention 'IEntityWithDiscriminator<>'");

            FillStringPropertyOptions(e, p, options);
        });
        return builder;
    }

    public static void FillStringPropertyOptions(EntityTypeBuilder e, PropertyBuilder p, StringPropertyOptions options)
    {
#pragma warning disable EF1001
        if (options.ColumnName is { } columnName && p.Metadata.FindAnnotation(RelationalAnnotationNames.ColumnName) is null)
        {
            p.HasColumnName(columnName);
        }
        if (p.Metadata.FindAnnotation(RelationalAnnotationNames.IsFixedLength) is null)
        {
            p.IsFixedLength(options.FixedLength);
        }
        if (p.Metadata.FindAnnotation(CoreAnnotationNames.MaxLength) is null)
        {
            p.HasMaxLength(options.MaxLength);
        }
        if (p.Metadata.FindAnnotation(CoreAnnotationNames.Unicode) is null)
        {
            p.IsUnicode(options.IsUnicode);
        }
#pragma warning restore EF1001
        FillPropertyOptions(e, p, options);
    }

    public static void FillPropertyOptions(EntityTypeBuilder e, PropertyBuilder p, BasePropertyOptions options)
    {
#pragma warning disable EF1001
        if (options.HasIndex)
        {
            var index = e.HasIndex(p.Metadata.Name);
            if (options.IndexIsUnique)
            {
                index.IsUnique();
            }
        }
#pragma warning restore EF1001
    }
}