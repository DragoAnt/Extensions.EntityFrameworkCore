using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityConventions.TriggerBased.Contacts;

/// <summary>
/// Entity with creation audited property Modified
/// </summary>
public interface IUpdateAuditedEntityConvention:IEntityConventionContract
{
    DateTime ModifiedAt => throw ExceptionHelper.ThrowRegistrationOnly();
}