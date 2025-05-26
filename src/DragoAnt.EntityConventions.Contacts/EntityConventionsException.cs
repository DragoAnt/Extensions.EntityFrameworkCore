using System.Runtime.Serialization;

namespace DragoAnt.EntityConventions.Contacts;

[Serializable]
public class EntityConventionsException : Exception
{
    public EntityConventionsException()
    {
    }

    public EntityConventionsException(string message) 
        : base(message)
    {
    }

    public EntityConventionsException(string message, Exception inner) 
        : base(message, inner)
    {
    }

    [Obsolete]
    protected EntityConventionsException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}