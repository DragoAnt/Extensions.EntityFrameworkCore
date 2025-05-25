namespace DragoAnt.Extensions.EntityConventions.Contacts.TriggerBased;

/// <summary>
/// Entity with creation audited property Modified
/// </summary>
public interface ISoftDeleteEntityConvention : IEntityConventionContract
{
    bool IsDeleted => throw ExceptionHelper.ThrowRegistrationOnly();
    DateTime? Deleted => throw ExceptionHelper.ThrowRegistrationOnly();
}