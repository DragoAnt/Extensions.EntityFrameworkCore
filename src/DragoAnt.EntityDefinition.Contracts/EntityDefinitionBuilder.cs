namespace DragoAnt.EntityDefinition.Contracts;

public sealed class EntityDefinitionRowBuilder : DefinitionBuilder<EntityDefinitionRow>
{
    private readonly DefinitionMap _map;

    /// <inheritdoc />
    public EntityDefinitionRowBuilder(DefinitionMap map, EntityDefinitionRow row)
        : base(row)
    {
        _map = map;
    }

    public DefinitionBuilder<PropertyDefinitionRow> AddProperty(string name, string? prefix = null)
    {
        var property = new PropertyDefinitionRow(ConcatenateName(prefix, name), _map.PropertyDefinitions.Count);
        Row.AddProperty(property);

        return new DefinitionBuilder<PropertyDefinitionRow>(property);
    }

    public static string ConcatenateName(string? chunk1, string chunk2)
    {
        if (string.IsNullOrEmpty(chunk1))
        {
            return chunk2;
        }
        if (string.IsNullOrEmpty(chunk2))
        {
            return chunk1;
        }
        return chunk1 + "." + chunk2;
    }
}