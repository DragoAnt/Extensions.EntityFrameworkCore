namespace DragoAnt.Extensions.EntityConventions.Contacts;

/// <summary>
/// Entity with creation audited property Created
/// </summary>
public interface IWithSourceSystemIdEntityConventionGuid : IWithSourceSystemIdEntityConvention
{
    Guid Id { get; }

    string IWithSourceSystemIdEntityConvention.GenerateSourceSystemId()
    {
        return Id.ToString("N");
    }
}