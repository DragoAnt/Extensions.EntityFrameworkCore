using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.TriggerBased.Contacts;
using DragoAnt.EntityFrameworkCore.Relational;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;

public static class ModelBuilderTriggerBasedConventionsExtensions
{
    public static void AddTriggerBasedCommonConventions(this IEntityConventionsBuilder builder)
    {
        builder
            .AddUpdateAudited()
            .AddSoftDelete();
    }

    public static IEntityConventionsBuilder AddUpdateAudited(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConventionPropertyTriggerBased<IUpdateAuditedEntityConvention>(x => x.ModifiedAt,
            (_, _, p) => p.IsRequired()
                .ValueGeneratedOnAddOrUpdate()
                .HasAnnotation(ConventionsAnnotationNames.SqlDefault_CurrentDateTime, true)
                .HasAnnotation(ConventionsAnnotationNames.ColumnTriggerUpdate_SqlDefault, true)
                .HasComment("Row last modified datetime. Updated by trigger. Configured by convention 'IUpdateAuditedEntity'"));

        return builder;
    }

    public static IEntityConventionsBuilder AddSoftDelete(this IEntityConventionsBuilder builder)
    {
        builder.AddInterfaceConventionTriggerBased<ISoftDeleteEntityConvention>(
            e =>
            {
                e.HasAnnotation(ConventionsAnnotationNames.SoftDelete, true);

                e.Property(typeof(bool), nameof(ISoftDeleteEntityConvention.IsDeleted))
                    .IsRequired()
                    .HasDefaultValue(false)
                    .HasComment(
                        "Row deleted flag. Used for soft delete row. Updated by 'instead of' trigger. Configured by convention 'ISoftDeleteEntityConvention'");

                e.Property(typeof(DateTime?), nameof(ISoftDeleteEntityConvention.Deleted))
                    .IsRequired(false)
                    .HasComment(
                        "Row deleted  datetime. Used for soft delete row. Updated by 'instead of' trigger. Configured by convention 'ISoftDeleteEntityConvention'");

                e.HasIndex(nameof(ISoftDeleteEntityConvention.IsDeleted));

                var entityParam = Expression.Parameter(e.Metadata.ClrType, "x");
                var deletedPropInfo = typeof(ISoftDeleteEntityConvention).GetProperty(nameof(ISoftDeleteEntityConvention.IsDeleted))!;
                var propAccess = Expression.MakeMemberAccess(entityParam, deletedPropInfo);
                var lambdaExpression = Expression.Lambda(
                    Expression.MakeBinary(ExpressionType.Equal, propAccess, Expression.Constant(false)),
                    entityParam);
                e.HasQueryFilter(lambdaExpression);
            });

        return builder;
    }

    public static void AddInterfaceConventionTriggerBased<TConvention>(this IEntityConventionsBuilder builder,
        Action<EntityTypeBuilder> configure, params string[]? triggerNames)
        where TConvention : IEntityConventionContract
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        if (triggerNames == null || triggerNames.Length == 0)
        {
            triggerNames = [typeof(TConvention).Name];
        }

        builder.AddInterfaceConvention<TConvention>(
            e =>
            {
                AddTriggersAnnotation(e, triggerNames);
                configure(e);
            });
    }

    public static void AddInterfaceConventionPropertyTriggerBased<TConvention>(this IEntityConventionsBuilder builder,
        Expression<Func<TConvention, object?>> propertyExpression,
        Action<EntityTypeBuilder, PropertyInfo, PropertyBuilder> configure, params string[]? triggerNames)
        where TConvention : IEntityConventionContract
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        if (triggerNames == null || triggerNames.Length == 0)
        {
            triggerNames = [typeof(TConvention).Name];
        }


        builder.AddInterfaceConventionProperty(propertyExpression,
            (e, pi, p) =>
            {
                AddTriggersAnnotation(e, triggerNames);
                configure(e, pi, p);
            });
    }

    private static void AddTriggersAnnotation(EntityTypeBuilder e, string[] triggerNames)
    {
        //NOTE: https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#sql-server-tables-with-triggers-or-certain-computed-columns-now-require-special-ef-core-configuration
#if NET7_0_OR_GREATER
        if (!e.Metadata.IsTable())
        {
            return;
        }
        foreach (var triggerName in triggerNames)
        {
            e.ToTable(tb => tb.HasTrigger(triggerName));
        }
#endif
    }
}