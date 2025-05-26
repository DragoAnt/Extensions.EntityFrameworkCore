namespace DragoAnt.EntityConventions.Contacts;

public static class ExceptionHelper
{
    /// <summary>
    /// Entity convention registration olny exception for avoid call registration only properties of convention
    /// </summary>
    /// <returns></returns>
    public static Exception ThrowRegistrationOnly() =>
        new NotSupportedException("This property exists for EF declaration only. For usage explicit declare it in inheritor");
}