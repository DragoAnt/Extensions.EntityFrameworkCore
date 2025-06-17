using DragoAnt.EntityConventions.Contacts;
using DragoAnt.EntityConventions.TriggerBased.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.Data.Main;
using DragoAnt.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.Migrations.Static;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.HistoricalMigrations.DependencyInjection;
using DragoAnt.EntityFrameworkCore.SqlServer.DependencyInjection;
using Main = DragoAnt.EntityFrameworkCore.Data.Main;
using MainWithoutAttribute = DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

namespace DragoAnt.EntityFrameworkCore.SqlServer.Tests;

public class HistoricalMigrationsRegistrationTests: TestBase
{
    private readonly MainDbContext _dbContextMain;
    private readonly MainWithoutAttribute.MainTypeRegistrationDbContext _dbContextMainTypeRegistration;
    private readonly MainWithoutAttribute.MainTypeRegistrationDbContext _dbContextMainTypeRegistrationChain;
    private readonly MainDbContext _dbContextMainTwoRegistrations;

    public HistoricalMigrationsRegistrationTests()
    {
        InitDbContext(MainStaticMigrations.Init,
            false,
            true,
            out _,
            out _dbContextMain);

        InitDbContext(MainWithoutAttributeStaticMigrations.Init,
            true,
            true,
            out _,
            out _dbContextMainTypeRegistration);

        InitDbContext(MainWithoutAttributeStaticMigrations.Init,
            true,
            true,
            out _,
            out _dbContextMainTypeRegistrationChain, typeof(MainDbContext_Step2));

        InitDbContext(MainStaticMigrations.Init,
            true,
            true,
            out _,
            out _dbContextMainTwoRegistrations);
    }

    private static void InitDbContext<TContext>(Action<StaticMigrationBuilder> init, bool useGenericRegistration, bool includeCommonConventions,
        out IServiceProvider serviceProvider,
        out TContext dbContext, Type? dbContextHistoryType = null)
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        serviceProvider = GetServices<TContext>(init, useGenericRegistration, includeCommonConventions, dbContextHistoryType: dbContextHistoryType);
        dbContext = serviceProvider.GetRequiredService<TContext>();
    }

    private static IServiceProvider GetServices<TDbContext>(Action<StaticMigrationBuilder> init, bool useGenericRegistration,
        bool includeCommonConventions, Type? dbContextHistoryType = null)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var services = new ServiceCollection();

        var dbName = GenerateUniqueDbName();
        var connectionString = GetConnectionString(dbName);

        services.AddDbContext<TDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
                builder.UseStaticMigrationsSqlServer(b =>
                {
                    init.Invoke(b);
                    if (includeCommonConventions)
                    {
                        b.AddTriggerBasedEntityConventionsMigrationSqlServer();
                    }
                });

                if (useGenericRegistration)
                {
                    if (dbContextHistoryType != null)
                    {
                        builder.UseHistoricalMigrations(options => options.DbContextType = dbContextHistoryType);
                    }
                    else
                    {
                        builder.UseHistoricalMigrations<MainDbContext_Step1PlusStep2>();
                    }
                }
                else
                {
                    builder.UseHistoricalMigrations();
                }

                if (includeCommonConventions)
                {
                    builder.UseEntityConventionsSqlServer(b => { b.AddTriggerBasedCommonConventions(); });
                }
            },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task EnsureCreated_Main()
    {
        await EnsureCreated(_dbContextMain);

        var actual = await _dbContextMain.Set<Main.Currency>().ToListAsync();
        var expected = Main.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMain.Set<Role>().OrderBy(x => x.Name).ToListAsync();
        var expectedRoles = Main.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles, options => options
            .Excluding(x => ((ICreateAuditedEntityConvention)x).Created)
            .Excluding(x => ((IUpdateAuditedEntityConvention)x).ModifiedAt)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).IsDeleted)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).Deleted));
    }

    [Fact]
    public async Task EnsureCreated_MainHistoricalWithoutAttribute()
    {
        await EnsureCreated(_dbContextMainTypeRegistration);

        var actual = await _dbContextMainTypeRegistration.Set<MainWithoutAttribute.Currency>().ToListAsync();
        var expected = MainWithoutAttribute.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMainTypeRegistration.Set<MainWithoutAttribute.Role>().OrderBy(x => x.Name).ToListAsync();
        var expectedRoles = MainWithoutAttribute.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles, options => options
            .Excluding(x => ((ICreateAuditedEntityConvention)x).Created)
            .Excluding(x => ((IUpdateAuditedEntityConvention)x).ModifiedAt)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).IsDeleted)
            .Excluding(x => ((ISoftDeleteEntityConvention)x).Deleted));
    }

    [Fact]
    public async Task Migrate_Main_Should_Run()
    {
        await RunMigrations(_dbContextMain, true);
    }

    [Fact]
    public async Task Migrate_MainTypeRegistration_Should_Run()
    {
        await RunMigrations(_dbContextMainTypeRegistration, true);
    }

    [Fact]
    public async Task Migrate_MainTypeRegistration_Chain_Should_Run()
    {
        await RunMigrations(_dbContextMainTypeRegistrationChain, true);
    }

    [Fact]
    public async Task Registering_Historic_Migrations_Both_Ways_Should_Raise_Error()
    {
        Func<Task> act = async () => await RunMigrations(_dbContextMainTwoRegistrations, true);
        await act.Should().ThrowAsync<NotSupportedException>().Where(e => e.Message.StartsWith("Use one of options"));
    }

    private static async Task RunMigrations(Microsoft.EntityFrameworkCore.DbContext dbContext, bool deleteDb = true)
    {
        var database = dbContext.Database;
        if (deleteDb)
        {
            await database.EnsureDeletedAsync();
        }
        await database.MigrateAsync();
    }

    private static async Task<bool> EnsureCreated(Microsoft.EntityFrameworkCore.DbContext dbContext, bool deleteDb = true)
    {
        var database = dbContext.Database;
        if (deleteDb)
        {
            await database.EnsureDeletedAsync();
        }
        return await database.EnsureCreatedAsync();
    }
}