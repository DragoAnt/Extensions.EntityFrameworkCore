namespace DragoAnt.EntityConventions.Contracts;

/// <summary>
/// Entity with creation audited property Created
/// </summary>
public interface ICreateAuditedEntityConvention : IEntityConventionContract
{
    DateTime Created => throw ExceptionHelper.ThrowRegistrationOnly();
}