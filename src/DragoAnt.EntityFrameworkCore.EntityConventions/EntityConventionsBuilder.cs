using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public class EntityConventionsBuilder : IEntityConventionsBuilder, IEntityConventionsService
{
    private readonly List<IEntityConvention> _conventions = new();

    public EntityConventionsBuilder(EntityConventionsCommonDefaultsOptions defaultOptions)
    {
        DefaultOptions = defaultOptions;
    }

    public EntityConventionsCommonDefaultsOptions DefaultOptions { get; }

        
    public void Add(IEntityConvention convention)
    {
        if (convention == null)
        {
            throw new ArgumentNullException(nameof(convention));
        }
        _conventions.Add(convention);
    }

    /// <inheritdoc />
    public void AddInterfaceConvention<TConvention>(Action<EntityTypeBuilder> configure)
        where TConvention:IEntityConventionContract
    {
        Add(new InterfaceConvention(typeof(TConvention), configure));
    }

    public void AddInterfaceConventionProperty<TConvention>(Expression<Func<TConvention, object?>> propertyExpression,
        Action<EntityTypeBuilder, PropertyInfo, PropertyBuilder> configure)
        where TConvention : IEntityConventionContract
    {
        var propInfo = (PropertyInfo)propertyExpression.GetMemberAccess();
        Add(new InterfaceConvention(typeof(TConvention),
            builder =>
            {
                var prop = builder.Property(propInfo.PropertyType, propInfo.Name);
                configure(builder, propInfo, prop);
            }));
    }

    /// <inheritdoc />
    public bool HasConventions => _conventions.Count > 0;

    public void Configure(EntityTypeBuilder entityBuilder)
    {
        foreach (var convention in _conventions.Where(c => c.Allowed(entityBuilder.Metadata)))
        {
            convention.Configure(entityBuilder);
        }
    }
}