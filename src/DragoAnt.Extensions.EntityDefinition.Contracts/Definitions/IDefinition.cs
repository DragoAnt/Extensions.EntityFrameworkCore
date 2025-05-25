namespace DragoAnt.Extensions.EntityDefinition.Contracts.Definitions;

public interface IDefinition
{
    /// <summary>
    /// Read definition order. Lower first 
    /// </summary>
    /// <returns></returns>
    int ReadOrder => 0;

    DefinitionInfo Info { get; }
}

public interface IDefinition<T> : IDefinition
{
    /// <inheritdoc />
    DefinitionInfo IDefinition.Info => Info;

    new DefinitionInfo<T> Info { get; }
}