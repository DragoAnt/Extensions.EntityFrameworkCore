namespace DragoAnt.Extensions.EntityConventions.Contacts.TriggerBased;

/// <summary>
/// Entity with creation audited property Modified
/// </summary>
public interface IUpdateAuditedEntityConvention:IEntityConventionContract
{
    DateTime ModifiedAt => throw ExceptionHelper.ThrowRegistrationOnly();
}