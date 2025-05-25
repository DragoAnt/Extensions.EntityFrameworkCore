using System.Diagnostics;

namespace DragoAnt.Extensions.EntityDefinition.Contracts;

[DebuggerDisplay("{Name}")]
public abstract class DefinitionInfo
{
    internal DefinitionInfo(string name)
    {
        Name = name;
    }

    public abstract Type ValueType { get; }

    public virtual string? ConvertToString(object? val)
    {
        return val?.ToString();
    }

    public string Name { get; }
}

public sealed class DefinitionInfo<T> : DefinitionInfo
{
    private readonly Func<T, string>? _convertToString;

    /// <inheritdoc />
    public DefinitionInfo(string name, Func<T, string>? convertToString = null)
        : base(name)
    {
        _convertToString = convertToString;
    }

    /// <inheritdoc />
    public override Type ValueType => typeof(T);

    /// <inheritdoc />
    public override string? ConvertToString(object? val)
    {
        if (_convertToString is not null && val is not null)
        {
            return _convertToString((T)val);
        }
        return base.ConvertToString(val);
    }
}