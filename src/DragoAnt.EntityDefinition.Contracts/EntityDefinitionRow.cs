namespace DragoAnt.EntityDefinition.Contracts;

public sealed class EntityDefinitionRow : DefinitionRowBase
{
    private readonly List<PropertyDefinitionRow> _properties;

    /// <inheritdoc />
    public EntityDefinitionRow(string name, int valuesCount)
        : base(name, valuesCount)
    {
        _properties = new List<PropertyDefinitionRow>();
    }

    public IReadOnlyCollection<PropertyDefinitionRow> Properties => _properties;

    internal void AddProperty(PropertyDefinitionRow property)
    {
        _properties.Add(property);
    }
}