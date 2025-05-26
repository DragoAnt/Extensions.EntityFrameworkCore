using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.Tests;

public static class EnumTableExtensions
{
    public static void RowShouldBe<T>(this EnumTableRow row, T value, string name, string displayName, bool isCombined = false)
    {
        ((T)row.Value).Should().Be(value);
        row.Name.Should().Be(name);
        row.DisplayName.Should().Be(displayName);
        row.IsCombined.Should().Be(isCombined);
    }
}