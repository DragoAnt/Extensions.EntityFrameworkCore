using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.Extensions.EntityConventions.Contacts;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public interface IEntityConventionsBuilder
{
    void Add(IEntityConvention convention);

    void AddInterfaceConvention<TConvention>(Action<EntityTypeBuilder> configure)
        where TConvention:IEntityConventionContract;
        
    void AddInterfaceConventionProperty<TConvention>(Expression<Func<TConvention, object?>> propertyExpression,
        Action<EntityTypeBuilder, PropertyInfo, PropertyBuilder> configure)
        where TConvention:IEntityConventionContract;
        
    EntityConventionsCommonDefaultsOptions DefaultOptions { get; }
}