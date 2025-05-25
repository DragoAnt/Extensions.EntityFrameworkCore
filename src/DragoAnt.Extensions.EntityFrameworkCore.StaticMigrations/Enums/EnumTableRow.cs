using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

[DebuggerDisplay("{Name}:{Value}")]
public class EnumTableRow
{
    public static EnumTableRow Create(Type enumType, Type valueType, MemberInfo member)
    {
        var name = member.Name;

        var enumValue = Enum.Parse(enumType, member.Name);
        var value = Convert.ChangeType(enumValue, valueType);
        var display = member.GetCustomAttribute<DisplayAttribute>();
        var displayName = member.GetCustomAttribute<DisplayNameAttribute>();
        var description = member.GetCustomAttribute<DescriptionAttribute>();

        return new EnumTableRow(value, enumValue, name,
            display?.GetName() ?? displayName?.DisplayName ?? name,
            display?.GetDescription() ?? description?.Description ?? string.Empty);
    }

    public static EnumTableRow CreateFromCombinedEnumValue<T>(Type valueType, T enumItem)
        where T : Enum
    {
        var name = SortCombinedEnumName(enumItem.ToString());
        var value = Convert.ChangeType(enumItem, valueType);
        var rawValue = value.ToString() ?? string.Empty;

        return new EnumTableRow(value, rawValue, name, name, string.Empty, true);
    }

    private static string SortCombinedEnumName(string name)
    {
        var parts = name.Split(',').Select(p => p.Trim()).ToList();
        parts.Sort();
        return string.Join(", ", parts);
    }

    private EnumTableRow(object value, object rawValue, string name, string displayName, string description, bool isCombined = false)
    {
        Value = value;
        RawValue = rawValue;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DisplayName = displayName;
        Description = description;
        IsCombined = isCombined;
    }

    public object Value { get; }
    public string Name { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public object RawValue { get; }
    public bool IsCombined { get; }
}