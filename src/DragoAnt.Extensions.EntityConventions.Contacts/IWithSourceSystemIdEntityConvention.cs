namespace DragoAnt.Extensions.EntityConventions.Contacts;

/// <summary>
/// Entity with source system id property
/// </summary>
public interface IWithSourceSystemIdEntityConvention : IEntityConventionContract
{
    /// <summary>
    /// Generate source system id
    /// </summary>
    /// <returns></returns>
    string GenerateSourceSystemId();

    string SourceSystemId => throw ExceptionHelper.ThrowRegistrationOnly();
}