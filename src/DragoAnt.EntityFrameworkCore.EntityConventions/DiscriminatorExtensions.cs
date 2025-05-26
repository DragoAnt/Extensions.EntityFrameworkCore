using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public static class DiscriminatorExtensions
{
    /// <summary>
    ///     Configures the discriminator property used to identify the entity type in the store.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TDiscriminator"> The type of values stored in the discriminator property. </typeparam>
    /// <param name="builder">Entity builder</param>
    /// <param name="init"> Descriminator's property initializing method </param>
    /// <returns> A builder that allows the discriminator property to be configured. </returns>
    public static DiscriminatorBuilder<TDiscriminator> HasConventionDiscriminator<TEntity, TDiscriminator>(this EntityTypeBuilder<TEntity> builder,
        Action<PropertyBuilder<TDiscriminator>>? init = null)
        where TEntity : class, IWithDiscriminatorEntityConvention<TDiscriminator>
    {
        const string propertyName = nameof(IWithDiscriminatorEntityConvention<TDiscriminator>.Discriminator);
        var property = builder.Property<TDiscriminator>(propertyName);
        init?.Invoke(property);
        return builder.HasDiscriminator<TDiscriminator>(propertyName);
    }
}