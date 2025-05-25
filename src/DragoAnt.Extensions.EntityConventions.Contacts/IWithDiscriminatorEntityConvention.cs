namespace DragoAnt.Extensions.EntityConventions.Contacts;

/// <summary>
/// Entity with discriminator for inheritance
/// </summary>
public interface IWithDiscriminatorEntityConvention<out T> : IWithDiscriminatorEntityConvention
{
    T Discriminator => throw ExceptionHelper.ThrowRegistrationOnly();
}

/// <summary>
/// Entity with discriminator for inheritance
/// </summary>
public interface IWithDiscriminatorEntityConvention : IEntityConventionContract
{
}