namespace DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

public abstract class Definition<T> : IDefinition<T>
{
    public static implicit operator DefinitionInfo<T>(Definition<T> v) => v.Info;
        
    protected Definition(string name, Func<T, string>? convertToString = null)
        : this(new DefinitionInfo<T>(name, convertToString))
    {
    }

    /// <inheritdoc />
    public int ReadOrder { get; init; } = 0;

    protected Definition(DefinitionInfo<T> info)
    {
        Info = info ?? throw new ArgumentNullException(nameof(info));
    }

    /// <inheritdoc />
    public DefinitionInfo<T> Info { get; }
}