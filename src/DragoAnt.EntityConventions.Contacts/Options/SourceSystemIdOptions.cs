namespace DragoAnt.EntityConventions.Contacts;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SourceSystemIdOptions : StringPropertyOptions
{
    /// <inheritdoc />
    public SourceSystemIdOptions()
    {
        IndexIsUnique = true;
        HasValueGenerator = true;
        Generator = null;

    }

    /// <summary>
    /// Has value generator
    /// </summary>
    public bool HasValueGenerator { get; set; }

    /// <summary>
    /// Value generator type if null standard value generator will be used
    /// </summary>
    public Type? Generator { get; set; }
}