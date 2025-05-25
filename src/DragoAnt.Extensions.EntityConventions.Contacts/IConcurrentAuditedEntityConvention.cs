namespace DragoAnt.Extensions.EntityConventions.Contacts;

/// <summary>
/// Entity with concurrent row version property
/// </summary>
public interface IConcurrentAuditedEntityConvention : IEntityConventionContract
{
    byte[] RowVersion => throw ExceptionHelper.ThrowRegistrationOnly();
}