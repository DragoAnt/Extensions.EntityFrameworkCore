using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.EntityFrameworkCore.Data.Initial;
using DragoAnt.EntityFrameworkCore.Data.Initial.DictEntities;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.EntityFrameworkCore.HistoricalMigrations.DependencyInjection;
using DragoAnt.StaticMigrations.MigrationConditions;

namespace DragoAnt.EntityFrameworkCore.SqlServer.Tests;

public class ConditionalStaticMigrationsTest : TestBase
{
    private static object[] CreateCondition(Func<StaticMigrationConditionOptions, bool> func) => [func];

    public static IEnumerable<object[]> ConditionOnName = [CreateCondition(x => x.ChangedMigrations.Any(i => i.Name.Contains("TestViews")))];
    public static IEnumerable<object[]> ConditionOnTag = [CreateCondition(x => x.ForcedRunActionTags.Any(i => i.Contains("vCurrencyTag")))];
    public static IEnumerable<object[]> NegativeConditionOnName = [CreateCondition(x => x.ChangedMigrations.Any(i => i.Name.Contains("NonExistingMagration")))];
    public static IEnumerable<object[]> NegativeConditionOnTag = [CreateCondition(x => x.ForcedRunActionTags.Any(i => i.Contains("NonExistingTag")))];

    private InitialDbContext _dbContextInitial = null!;

    public static Action<StaticMigrationBuilder> BuildInit(Func<StaticMigrationConditionOptions, bool>? condition = null)
    {
        Action<StaticMigrationBuilder> action = migrations =>
        {
            migrations.AddInitialSqlResFile("InitDB", suppressTransaction: true, typeof(InitialDbContext).Assembly);

            migrations.AddSqlResFile("TestViews", ResSqlFile.All, typeof(InitialDbContext).Assembly);
            migrations.AddSqlResFile("vCurrency", ResSqlFile.All, typeof(InitialDbContext).Assembly, condition);
        };

        return action;
    }

    [Theory]
    [InlineData(null)]
    [MemberData(nameof(ConditionOnName))]
    public async Task MigrationsAfterCreateShouldCreateTableAndView(Func<StaticMigrationConditionOptions, bool>? condition = null)
    {
        InitDbContext(BuildInit(condition), false,
            out _,
            out _dbContextInitial);

        await EnsureCreated(_dbContextInitial);

        await VerifyConditionalMigrationsExecuted();
    }

    [Theory]
    [MemberData(nameof(ConditionOnTag))]
    public async Task MigrationsAfterMigrateShouldCreateTableAndView(Func<StaticMigrationConditionOptions, bool>? condition = null)
    {
        InitDbContext(BuildInit(condition), false,
            out _,
            out _dbContextInitial);

        await RunMigrations(_dbContextInitial); // need to run migrations here, calling EnsureCreated does not initialize tags

        await VerifyConditionalMigrationsExecuted();
    }

    [Theory]
    [MemberData(nameof(NegativeConditionOnName))]
    [MemberData(nameof(NegativeConditionOnTag))]
    public async Task MigrationsWithNegativeConditionShouldNotCreateView(Func<StaticMigrationConditionOptions, bool>? condition = null)
    {
        InitDbContext(BuildInit(condition), false,
            out _,
            out _dbContextInitial);

        await EnsureCreated(_dbContextInitial);

        await VerifyConditionalMigrationsNotExecuted();
    }

    private async Task VerifyConditionalMigrationsExecuted()
    {
        var actual = await _dbContextInitial.Set<CurrencyV1>().ToListAsync();
        var expected = CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualView = await _dbContextInitial.Set<VCurrency>().ToListAsync();
        actualView.Count.Should().Be(expected.Count);
    }

    private async Task VerifyConditionalMigrationsNotExecuted()
    {
        var actual = await _dbContextInitial.Set<CurrencyV1>().ToListAsync();
        var expected = CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        Action getVCurrency = () => _dbContextInitial.Set<VCurrency>().ToList();

        getVCurrency.Should()
            .Throw<Microsoft.Data.SqlClient.SqlException>()
            .WithMessage("Invalid object name 'vCurrency'.");
    }

    private static void InitDbContext<TContext>(
        Action<StaticMigrationBuilder> init,
        bool includeCommonConventions,
        out IServiceProvider serviceProvider,
        out TContext dbContext)
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        serviceProvider = GetServices<TContext>(init, includeCommonConventions);
        dbContext = serviceProvider.GetRequiredService<TContext>();
    }

    private static IServiceProvider GetServices<TDbContext>(
        Action<StaticMigrationBuilder> init,
        bool includeCommonConventions)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var dbName = GenerateUniqueDbName();
        var connectionString = GetConnectionString(dbName);

        var services = new ServiceCollection();

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

                builder.UseHistoricalMigrations();

                if (includeCommonConventions)
                {
                    builder.UseEntityConventionsSqlServer(b => { b.AddTriggerBasedCommonConventions(); });
                }
            },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services.BuildServiceProvider();
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