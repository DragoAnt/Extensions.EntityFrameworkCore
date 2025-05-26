namespace DragoAnt.EntityConventions.Contacts;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CreateAuditedOptions : BasePropertyOptions
{
    /// <inheritdoc />
    public CreateAuditedOptions()
    {
        IndexIsUnique = false;
        HasIndex = false;
            
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