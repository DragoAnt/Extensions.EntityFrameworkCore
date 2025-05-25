namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

[AttributeUsage(AttributeTargets.Enum)]
public sealed class EnumTableAttribute : Attribute
{
    /// <inheritdoc />
    public EnumTableAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public Type? ValueType { get; init; }
}