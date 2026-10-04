using DragoAnt.EntityConventions.Contracts;

namespace DragoAnt.EntityConventions.TriggerBased.Contracts;

/// <summary>
/// Entity with creation audited property Modified
/// </summary>
public interface IUpdateAuditedEntityConvention:IEntityConventionContract
{
    DateTime ModifiedAt => throw ExceptionHelper.ThrowRegistrationOnly();
}