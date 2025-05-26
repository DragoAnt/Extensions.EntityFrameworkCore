namespace DragoAnt.EntityDefinition.Contracts;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property)]
public abstract class DefinitionAttribute : Attribute
{
    protected DefinitionAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public object Value { get; }
}