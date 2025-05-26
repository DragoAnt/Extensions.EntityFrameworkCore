using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityDefinition.EntityFrameworkCore;
using DragoAnt.EntityDefinition.EntityFrameworkCore.Definitions;
using DragoAnt.EntityDefinition.Model;
using DragoAnt.EntityDefinition.Model.Definitions;
using DragoAnt.EntityFrameworkCore.Relational;

namespace DragoAnt.EntityDefinition.InMemory.Tests;

public class EntityDefinitionsTests
{
    private const string DbName = "stenn_efcore_tests_in_memory";
    private readonly DefinitionDbContext _dbContext;

    public EntityDefinitionsTests()
    {
        var serviceProvider = GetServices<DefinitionDbContext>();
        _dbContext = serviceProvider.GetRequiredService<DefinitionDbContext>();
    }

    private static IServiceProvider GetServices<TDbContext>()
        where TDbContext : DbContext
    {
        var services = new ServiceCollection();

        services.AddDbContext<TDbContext>(builder => { builder.UseInMemoryDatabase(DbName); });

        return services.BuildServiceProvider();
    }

    [Fact]
    public void TestEntities()
    {
        var csv = _dbContext.Model.GenerateCsv(options =>
        {
            options.AddCommonConvert<bool>(CommonDefinitions.Converts.BoolToX);

            options.AddEntityColumn(CustomDefinitions.Domain.ToEntity());
            options.AddEntityColumn(EFCommonDefinitions.Entities.Name);
            options.AddEntityColumn(EFCommonDefinitions.Entities.Remark);
            options.AddEntityColumn(EFCommonDefinitions.Entities.IsObsolete);
            options.AddEntityColumn(EFCommonDefinitions.Entities.GetXmlDescription());
        });
    }

    [Fact]
    public void TestProperties()
    {
        var csv = _dbContext.Model.GenerateCsv(options =>
        {
            options.AddCommonConvert<bool>(CommonDefinitions.Converts.BoolToX);

            options.AddEntityColumn(CustomDefinitions.Domain.ToEntity(), "Entity:Domain");
            options.AddPropertyColumn(CustomDefinitions.Domain.ToProperty());
            options.AddPropertyColumn(CustomDefinitions.IsDomainDifferent.ToProperty());

            options.AddEntityColumn(EFCommonDefinitions.Entities.Name, "Entity:Name");
            options.AddPropertyColumn(EFCommonDefinitions.Properties.Name);
            options.AddPropertyColumn(EFCommonDefinitions.Properties.Remark);
            options.AddPropertyColumn(EFCommonDefinitions.Properties.IsObsolete);
            options.AddPropertyColumn(EFCommonDefinitions.Properties.IsShadow);
            options.AddPropertyColumn(EFCommonDefinitions.Properties.GetXmlDescription());
        });
    }


    [Theory]
    [InlineData(typeof(Invoice), true)]
    [InlineData(typeof(InvoiceView), false)]
    [InlineData(typeof(InvoiceViewExtended), false)]
    [InlineData(typeof(User), true)]
    [InlineData(typeof(StandardUser), true)]
    [InlineData(typeof(SuperUser), true)]
    [InlineData(typeof(Role), true)]
    [InlineData(typeof(UserRole), true)]
    public void CheckEntityType(Type type, bool isTable)
    {
        var isView = !isTable;
        var context = _dbContext;

        var entityType = context.Model.GetEntityTypes().First(t => t.ClrType == type);

        entityType.IsView().Should().Be(isView);
        entityType.IsTable().Should().Be(isTable);
    }
}