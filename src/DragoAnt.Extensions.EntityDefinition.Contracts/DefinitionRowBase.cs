using System.Diagnostics;

namespace DragoAnt.Extensions.EntityDefinition.Contracts;

[DebuggerDisplay("{Name}")]
public abstract class DefinitionRowBase
{
    protected DefinitionRowBase(string name, int valuesCount)
    {
        Name = name;
        Values = new Dictionary<DefinitionInfo, object?>(valuesCount);
    }

    public string Name { get; }
    internal Dictionary<DefinitionInfo, object?> Values { get; }

    public bool TryGetValue<T>(DefinitionInfo<T> info, out T? val)
    {
        var ret = TryGetValue((DefinitionInfo)info, out var valObj);
        val = valObj == null ? default : (T?)valObj;
        return ret;
    }

    public bool TryGetValue(DefinitionInfo info, out object? val)
    {
        return Values.TryGetValue(info, out val);
    }

    public T? Get<T>(DefinitionInfo<T> info)
    {
        var val = Get((DefinitionInfo)info);
        if (val == null)
        {
            return default;
        }
        return (T?)val;
    }

    public object? Get(DefinitionInfo info)
    {
        if (!Values.TryGetValue(info, out var val))
        {
            throw new ArgumentOutOfRangeException(nameof(info), $"Can't find value for '{info}'");
        }
        return val;
    }

    public T? GetValueOrDefault<T>(DefinitionInfo<T> info)
    {
        var val = GetValueOrDefault((DefinitionInfo)info);
        if (val == null)
        {
            return default;
        }
        return (T?)val;
    }

    public object? GetValueOrDefault(DefinitionInfo info)
    {
        Values.TryGetValue(info, out var val);
        return val;
    }
}