using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityConventions.TriggerBased.Contacts;

/// <summary>
/// Entity with creation audited property Modified
/// </summary>
public interface ISoftDeleteEntityConvention : IEntityConventionContract
{
    bool IsDeleted => throw ExceptionHelper.ThrowRegistrationOnly();
    DateTime? Deleted => throw ExceptionHelper.ThrowRegistrationOnly();
}