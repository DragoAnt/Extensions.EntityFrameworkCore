namespace DragoAnt.EntityConventions.Contracts;

/// <summary>
/// Entity with concurrent row version property
/// </summary>
public interface IConcurrentAuditedEntityConvention : IEntityConventionContract
{
    byte[] RowVersion => throw ExceptionHelper.ThrowRegistrationOnly();
}