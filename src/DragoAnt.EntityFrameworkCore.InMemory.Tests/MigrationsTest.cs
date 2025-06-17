using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.TriggerBased.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.Data.Initial;
using DragoAnt.EntityFrameworkCore.Data.Initial.DictEntities;
using DragoAnt.EntityFrameworkCore.Data.Initial.Migrations.Static;
using DragoAnt.EntityFrameworkCore.Data.Main;
using DragoAnt.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.InMemory.DependencyInjection;
using DragoAnt.EntityFrameworkCore.Testing;

namespace DragoAnt.EntityFrameworkCore.InMemory.Tests;

public class MigrationsTest
{
    private const string DbName = "efcore_tests_in_memory";
    private readonly InitialDbContext _dbContextInitial;

    private readonly MainDbContext _dbContextMain;

    public MigrationsTest()
    {
        var serviceProviderInitial = GetServices<InitialDbContext>(InitialStaticMigrations.Init);
        _dbContextInitial = serviceProviderInitial.GetRequiredService<InitialDbContext>();

        var serviceProviderMain = GetServices<MainDbContext>(MainStaticMigrations.Init);
        _dbContextMain = serviceProviderMain.GetRequiredService<MainDbContext>();
    }

    private static IServiceProvider GetServices<TDbContext>(Action<StaticMigrationBuilder> init)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var services = new ServiceCollection();

        services.AddDbContext<TDbContext>(builder =>
        {
            builder.UseInMemoryDatabase(DbName);
            builder.UseStaticMigrationsInMemoryDatabase(init);
        });

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task EnsureCreated_Initial()
    {
        await EnsureCreated(_dbContextInitial);

        var actual = await _dbContextInitial.Set<CurrencyV1>().ToListAsync();
        var expected = CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task EnsureCreated_Main()
    {
        await EnsureCreated(_dbContextMain);

        var actual = await _dbContextMain.Set<Currency>().ToListAsync();
        var expected = Data.Main.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);
            
        var actualRoles = await _dbContextMain.Set<Role>().ToListAsync();
        var expectedRoles = Data.Main.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles, options => options
            .Excluding(x => ((ICreateAuditedEntityConvention)x).Created)
            .Excluding(x => ((IUpdateAuditedEntityConvention)x).ModifiedAt)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).IsDeleted)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).Deleted));
    }

    [Fact]
    public void CheckMapping()
    {
        _dbContextInitial.CheckEntities();
        _dbContextMain.CheckEntities();
    }

    private static async Task EnsureCreated(Microsoft.EntityFrameworkCore.DbContext dbContext, bool deleteDb = true)
    {
        var database = dbContext.Database;
        if (deleteDb)
        {
            await database.EnsureDeletedAsync();
        }
        await database.EnsureCreatedAsync();
    }
}