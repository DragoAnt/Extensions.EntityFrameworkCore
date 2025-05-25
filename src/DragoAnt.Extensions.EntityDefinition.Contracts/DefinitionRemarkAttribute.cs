namespace DragoAnt.Extensions.EntityDefinition.Contracts;

/// <summary>
/// Definition remark attribute
/// </summary>
public sealed class DefinitionRemarkAttribute : DefinitionAttribute
{
    /// <inheritdoc />
    public DefinitionRemarkAttribute(string value) 
        : base("Remark", value)
    {
    }
}