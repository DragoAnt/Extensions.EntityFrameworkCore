using DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;
using DragoAnt.Extensions.EntityFrameworkCore.Relational;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests;

public class EntityTypeExtensionsTests
{
    [Fact]
    public void IsView()
    {
        var context = new InitialDbContextFactory().CreateDbContext([]);

        var currency = context.Model.GetEntityTypes().First(t => t.ClrType == typeof(CurrencyV1));
        currency.IsView().Should().BeFalse();
            
        var vcurrency = context.Model.GetEntityTypes().First(t => t.ClrType == typeof(VCurrency));
        vcurrency.IsView().Should().BeTrue();
    }

    [Fact]
    public void IsTable()
    {
        var context = new InitialDbContextFactory().CreateDbContext([]);

        var currency = context.Model.GetEntityTypes().First(t => t.ClrType == typeof(CurrencyV1));
        currency.IsTable().Should().BeTrue();
            
        var vcurrency = context.Model.GetEntityTypes().First(t => t.ClrType == typeof(VCurrency));
        vcurrency.IsTable().Should().BeFalse();
    }
}