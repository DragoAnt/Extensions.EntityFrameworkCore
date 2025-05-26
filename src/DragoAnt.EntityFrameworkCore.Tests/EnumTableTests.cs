using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;
using System.ComponentModel.DataAnnotations;

namespace DragoAnt.EntityFrameworkCore.Tests;

public class EnumTableTests
{
    [EnumTable("TestName", ValueType = typeof(short))]
    public enum EnumByte : byte
    {
        [Display(Name = "Default value")] 
        None = 0,
        [Display(Name = "First item")] 
        One = 1,
        [Display(Name = "Second value")] 
        Two = 2
    }

    [Fact]
    public void ParseEnumByte()
    {
        var table = EnumTable.Create<EnumByte>();

        table.EnumType.Should().Be<EnumByte>();
        table.ValueType.Should().Be<short>();
        table.Rows.Should().HaveCount(3);

        table.Rows[0].RowShouldBe<short>(0, "None", "Default value");
        table.Rows[1].RowShouldBe<short>(1, "One", "First item");
        table.Rows[2].RowShouldBe<short>(2, "Two", "Second value");
    }
        
    public enum EnumDefault
    {
        First = 1,
        Second = 2
    }

    [Fact]
    public void ParseEnumDefault()
    {
        var table = EnumTable.Create<EnumDefault>();

        table.EnumType.Should().Be<EnumDefault>();
        table.ValueType.Should().Be<int>();
        table.Rows.Should().HaveCount(2);

        table.Rows[0].RowShouldBe(1, "First", "First");
        table.Rows[1].RowShouldBe(2, "Second", "Second");
    }

    [Flags]
    public enum FlagsValue
    {
        One = 0x1,
        Two = 0x2,
        OneAndTwo = One | Two,
        Four = 0x10
    }

    /// <summary>
    /// Expected values:
    /// One, 
    /// Two,
    /// OneAndTwo, // Skip One | Two due existed explicit combination
    /// Four
    /// One | Four
    /// Two | Four
    /// OneAndTwo | Four // Skip One | Two | Four due existed combination with fewer values
    /// </summary>
    [Fact]
    public void ParseFlaggedEnum()
    {
        var table = EnumTable.Create<FlagsValue>();

        table.EnumType.Should().Be<FlagsValue>();
        table.ValueType.Should().Be(typeof(FlagsValue).GetEnumUnderlyingType());

        table.Rows.Should().HaveCount(7);

        table.Rows[0].RowShouldBe(FlagsValue.Four, "Four", "Four");
        table.Rows[1].RowShouldBe(FlagsValue.Four | FlagsValue.One, "Four, One", "Four, One", true);
        table.Rows[2].RowShouldBe(FlagsValue.Four | FlagsValue.OneAndTwo, "Four, OneAndTwo", "Four, OneAndTwo", true);
        table.Rows[3].RowShouldBe(FlagsValue.Four | FlagsValue.Two, "Four, Two", "Four, Two", true);
        table.Rows[4].RowShouldBe(FlagsValue.One, "One", "One");
        table.Rows[5].RowShouldBe(FlagsValue.OneAndTwo, "OneAndTwo", "OneAndTwo");
        table.Rows[6].RowShouldBe(FlagsValue.Two, "Two", "Two");
    }

    [Flags]
    public enum FlagsValueByte: byte
    {
        One = 0x1,
        Two = 0x2,
        OneAndTwo = One | Two,
        Four = 0x10
    }
        
    /// <summary>
    /// Expected values:
    /// One, 
    /// Two,
    /// OneAndTwo, // Skip One | Two due existed explicit combination
    /// Four
    /// One | Four
    /// Two | Four
    /// OneAndTwo | Four // Skip One | Two | Four due existed combination with fewer values
    /// </summary>
    [Fact]
    public void ParseFlaggedEnumByte()
    {
        var table = EnumTable.Create<FlagsValueByte>();

        table.EnumType.Should().Be<FlagsValueByte>();
        table.ValueType.Should().Be(typeof(FlagsValueByte).GetEnumUnderlyingType());

        table.Rows.Should().HaveCount(7);

        table.Rows[0].RowShouldBe(FlagsValueByte.Four, "Four", "Four");
        table.Rows[1].RowShouldBe(FlagsValueByte.Four | FlagsValueByte.One, "Four, One", "Four, One", true);
        table.Rows[2].RowShouldBe(FlagsValueByte.Four | FlagsValueByte.OneAndTwo, "Four, OneAndTwo", "Four, OneAndTwo", true);
        table.Rows[3].RowShouldBe(FlagsValueByte.Four | FlagsValueByte.Two, "Four, Two", "Four, Two", true);
        table.Rows[4].RowShouldBe(FlagsValueByte.One, "One", "One");
        table.Rows[5].RowShouldBe(FlagsValueByte.OneAndTwo, "OneAndTwo", "OneAndTwo");
        table.Rows[6].RowShouldBe(FlagsValueByte.Two, "Two", "Two");
    }

    [Flags]
    public enum FlagsValueByteNone : byte
    {
        None = 0,
        One = 0x10,
        Two = 0x20,
        Four = 0x80,

        OneAndTwo = One | Two,
        All = One | Two | Four
    }

    [Fact]
    public void ParseFlaggedEnumByteWithNone()
    {
        var table = EnumTable.Create<FlagsValueByteNone>();

        table.EnumType.Should().Be<FlagsValueByteNone>();
        table.ValueType.Should().Be(typeof(FlagsValueByteNone).GetEnumUnderlyingType());

        table.Rows.Should().HaveCount(8);

        table.Rows[0].RowShouldBe(FlagsValueByteNone.All, "All", "All");
        table.Rows[1].RowShouldBe(FlagsValueByteNone.Four, "Four", "Four");
        table.Rows[2].RowShouldBe(FlagsValueByteNone.Four | FlagsValueByteNone.One, "Four, One", "Four, One", true);
        table.Rows[3].RowShouldBe(FlagsValueByteNone.Four | FlagsValueByteNone.Two, "Four, Two", "Four, Two", true);
        table.Rows[4].RowShouldBe(FlagsValueByteNone.None, "None", "None");
        table.Rows[5].RowShouldBe(FlagsValueByteNone.One, "One", "One");
        table.Rows[6].RowShouldBe(FlagsValueByteNone.OneAndTwo, "OneAndTwo", "OneAndTwo");
        table.Rows[7].RowShouldBe(FlagsValueByteNone.Two, "Two", "Two");
    }
}