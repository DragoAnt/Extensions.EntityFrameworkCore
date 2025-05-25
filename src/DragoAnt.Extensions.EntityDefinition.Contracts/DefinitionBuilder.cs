namespace DragoAnt.Extensions.EntityDefinition.Contracts;

public class DefinitionBuilder<T>
    where T : DefinitionRowBase
{
    public DefinitionBuilder(T row)
    {
        Row = row;
    }

    public T Row { get; }

    public void AddDefinition(DefinitionInfo info, object? value)
    {
        Row.Values.Add(info, value);
    }
}