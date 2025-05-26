namespace DragoAnt.EntityDefinition.Contracts;

public sealed class PropertyDefinitionRow : DefinitionRowBase
{
    /// <inheritdoc />
    public PropertyDefinitionRow(string name, int valuesCount)
        : base(name, valuesCount)
    {
    }
}