using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Initial.Migrations.Static;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.EF6Initial;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalInitial;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.SqlServer.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;
using DragoAnt.Extensions.EntityFrameworkCore.Tests;

namespace DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Tests;

public class MigrationsTest: TestBase
{

    private readonly InitialDbContext _dbContextInitial;
    private readonly MainDbContext _dbContextMain;
    private readonly HistoricalInitialMainDbContext _dbContextMainHistoricalInitial;
    private readonly EF6InitialMainDbContext _dbContextMainEF6Initial;

    private IServiceProvider _serviceProviderInitial;
    private readonly IServiceProvider _serviceProviderMain;
    private IServiceProvider _serviceProviderMainHistoricalInitial;
    private IServiceProvider _serviceProviderMainEF6Initial;

        

    public MigrationsTest()
    {
        InitDbContext(InitialStaticMigrations.Init, false,
            out _serviceProviderInitial,
            out _dbContextInitial);

        InitDbContext(MainStaticMigrations.Init, true,
            out _serviceProviderMain,
            out _dbContextMain);

        InitDbContext(MainStaticMigrations.Init, true,
            out _serviceProviderMainHistoricalInitial,
            out _dbContextMainHistoricalInitial);

        InitDbContext(MainStaticMigrations.Init, true,
            out _serviceProviderMainEF6Initial,
            out _dbContextMainEF6Initial);
    }

    private static void InitDbContext<TContext>(Action<StaticMigrationBuilder> init, bool includeCommonConventions,
        out IServiceProvider serviceProvider,
        out TContext dbContext)
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        serviceProvider = GetServices<TContext>(init, includeCommonConventions);
        dbContext = serviceProvider.GetRequiredService<TContext>();
    }

    private static IServiceProvider GetServices<TDbContext>(Action<StaticMigrationBuilder> init,
        bool includeCommonConventions, string dbName = DbName)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var services = new ServiceCollection();

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

                builder.UseHistoricalMigrations();

                if (includeCommonConventions)
                {
                    builder.UseEntityConventionsSqlServer(b => { b.AddTriggerBasedCommonConventions(); });
                }
            },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task EnsureCreated_Initial()
    {
        await EnsureCreated(_dbContextInitial);

        var actual = await _dbContextInitial.Set<CurrencyV1>().ToListAsync();
        var expected = Data.Initial.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task EnsureCreated_Main()
    {
        await EnsureCreated(_dbContextMain);

        var actual = await _dbContextMain.Set<Currency>().ToListAsync();
        var expected = Data.Main.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMain.Set<Role>().ToListAsync();
        var expectedRoles = Data.Main.StaticMigrations.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task EnsureCreated_MainHistoricalInitial()
    {
        await EnsureCreated(_dbContextMainHistoricalInitial);

        var actual = await _dbContextMainHistoricalInitial.Set<Currency>().ToListAsync();
        var expected = Data.Main.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMainHistoricalInitial.Set<Role>().ToListAsync();
        var expectedRoles = Data.Main.StaticMigrations.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task EnsureCreated_MainEF6Initial()
    {
        await EnsureCreated(_dbContextMainEF6Initial);

        var actual = await _dbContextMainEF6Initial.Set<Currency>().ToListAsync();
        var expected = Data.Main.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMainEF6Initial.Set<Role>().ToListAsync();
        var expectedRoles = Data.Main.StaticMigrations.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles);
    }


    [Fact]
    public async Task Migrate_Initial()
    {
        await RunMigrations(_dbContextInitial);

        await CheckSelect(_dbContextInitial, "SELECT * FROM dbo.ContactView",
            async reader =>
            {
                var count = 0;
                while (await reader.ReadAsync())
                {
                    count++;
                    reader.GetInt32(0).Should().Be(1);
                }
                count.Should().Be(1);
            });


        var actual = await _dbContextInitial.Set<CurrencyV1>().ToListAsync();
        var expected = Data.Initial.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Migrate_Main()
    {
        await Migrate_MainTestRunner(true);
    }

    [Fact]
    public async Task Migrate_MainHistoricalInitial()
    {
        await Migrate_MainHistoricalInitialTestRunner(true);
    }

    [Fact]
    public async Task Migrate_MainEF6Initial()
    {
        await Migrate_MainEF6InitialTestRunner(true);
    }

    [Fact(Explicit = true)]
    public async Task Migrate_MainWithoutDeletion()
    {
        await Migrate_MainTestRunner(false);
    }

    [Fact]
    public async Task Migrate_InitialThenMain()
    {
        await Migrate_Initial();
        await Migrate_MainTestRunner(false);
    }

    [Fact]
    public async Task Migrate_InitialThenMainThenMainHistoricalInitial()
    {
        await Migrate_Initial();
        await Migrate_MainTestRunner(false);
        await Migrate_MainHistoricalInitialTestRunner(false);
    }

    [Fact]
    public async Task Migrate_InitialThenMain_EmulateEF6_ThenMainEF6Initial()
    {
        await Migrate_Initial();
        await Migrate_MainTestRunner(false);
        await EmulateEF6();
        await Migrate_MainEF6InitialTestRunner(false);
    }
        
    [Fact]
    public async Task Migrate_InitialThenMain_EmulateEF6_ThenMainEF6Initial_ThenMainEF6Initial()
    {
        await Migrate_Initial();
        await Migrate_MainTestRunner(false);
        await EmulateEF6();
        await Migrate_MainEF6InitialTestRunner(false);
        //Emulate run migration after migrate from EF6
        await Migrate_MainEF6InitialTestRunner(false);
    }

    private async Task EmulateEF6()
    {
        //NOTE: Convert __EFMigrationsHistory(Core) to __MigrationHistory(EF6) 
        //EF6Migration uses only MigrationId column, so we can just rename original __EFMigrationsHistory to __MigrationHistory
        await _dbContextMain.Database.ExecuteSqlRawAsync(
            @"
DROP TABLE IF EXISTS [dbo].[__MigrationHistory]

CREATE TABLE [dbo].[__MigrationHistory] (
[MigrationId] nvarchar(150) NOT NULL,
[ContextKey] nvarchar(300) NOT NULL DEFAULT('efcore-tests'),
[Model] varbinary(MAX) NOT NULL DEFAULT (0xFF),
[ProductVersion] nvarchar(32) NOT NULL,
CONSTRAINT [PK_dbo.__MigrationHistory]
PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC))

INSERT INTO [dbo].[__MigrationHistory](MigrationId, ProductVersion)
SELECT MigrationId, '6.4.4'
FROM [dbo].[__EFMigrationsHistory]
    
DROP TABLE IF EXISTS [dbo].[__EFMigrationsHistory]
");
    }

    private async Task Migrate_MainTestRunner(bool deleteDb)
    {
        await RunMigrations(_dbContextMain, deleteDb);

        await CheckMain(_dbContextMain);
    }

    private async Task Migrate_MainHistoricalInitialTestRunner(bool deleteDb)
    {
        await RunMigrations(_dbContextMainHistoricalInitial, deleteDb);

        await CheckMain(_dbContextMainHistoricalInitial);
    }

    private async Task Migrate_MainEF6InitialTestRunner(bool deleteDb)
    {
        await RunMigrations(_dbContextMainEF6Initial, deleteDb);

        await CheckMain(_dbContextMainEF6Initial);
    }

    private async Task CheckMain(Microsoft.EntityFrameworkCore.DbContext context)
    {
        await CheckSelect(context, "SELECT * FROM dbo.ContactView",
            async reader =>
            {
                var count = 0;
                while (await reader.ReadAsync())
                {
                    count++;
                    reader.GetInt32(0).Should().Be(2);
                }
                count.Should().Be(1);
            });

        var actual = await _dbContextMain.Set<Currency>().ToListAsync();
        var expected = Data.Main.StaticMigrations.DictEntities.CurrencyDeclaration.GetActual();
        actual.Should().BeEquivalentTo(expected);

        var actualRoles = await _dbContextMain.Set<Role>().ToListAsync();
        var expectedRoles = Data.Main.StaticMigrations.DictEntities.RoleDeclaration.GetActual();
        actualRoles.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public void ExtractEnums_Main()
    {
        var enumTables = _dbContextMain.Model.ExtractEnumTables().ToList();

        enumTables.Should().HaveCount(5);

        var table = enumTables.First().Table;

        table.EnumType.Should().Be<ContactType>();
        table.ValueType.Should().Be<byte>();
        table.Rows.Should().HaveCount(2);

        table.Rows[0].RowShouldBe((byte)2, "Organization", "Organization contact");
        table.Rows[1].RowShouldBe((byte)1, "Person", "Person contact");
    }

    [Fact]
    public async Task SoftDeleteRoles()
    {
        await EnsureCreated_Main();

        var originCount = _dbContextMain.Set<Role>().Count();

        var newRoleDeleteId = Guid.NewGuid();
        var roleForDelete = Role.Create(newRoleDeleteId.ToString(), "ForDelete");
        await _dbContextMain.Set<Role>().AddAsync(roleForDelete);
        await _dbContextMain.SaveChangesAsync();

        var afterAddCount = _dbContextMain.Set<Role>().Count();
        afterAddCount.Should().Be(originCount + 1);

        await Task.Delay(1000);
        var modifiedRole = await _dbContextMain.Set<Role>().FirstAsync(x => x.Name == "Customer");
        modifiedRole.Description = "MODIFIED DESC";
        await _dbContextMain.SaveChangesAsync();

        var roleToDelete = await _dbContextMain.Set<Role>().SingleAsync(x => x.Id == newRoleDeleteId);
        _dbContextMain.Set<Role>().Remove(roleToDelete);
        await _dbContextMain.SaveChangesAsync();

        var afterRemoveCount = _dbContextMain.Set<Role>().Count();
        afterRemoveCount.Should().Be(originCount);
        //NOTE: Check that deleted row exists in db(Soft delete)
        await CheckSelect(_dbContextMain, "SELECT * FROM dbo.Role",
            async reader =>
            {
                var count = 0;
                while (await reader.ReadAsync())
                {
                    count++;
                }
                count.Should().Be(afterAddCount);
            });
    }

    [Theory]
    [InlineData(1000)]
    public async Task CreateMultipleRequests(int countOfParallelRequests)
    {
        await Migrate_Main();
        Enumerable.Range(0, countOfParallelRequests).AsParallel().ForAll(i =>
        {
            using var scope = _serviceProviderMain.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            var currencies = dbContext.Set<Currency>().ToList();
            Console.WriteLine($"Id: {i}, ContextId:{dbContext.ContextId.InstanceId}, Count:{currencies.Count}");
        });
    }

    private static async Task CheckSelect(Microsoft.EntityFrameworkCore.DbContext context, string selectCommand, Func<DbDataReader, Task> check)
    {
        await using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = selectCommand;
        await context.Database.OpenConnectionAsync();
        await using var reader = await command.ExecuteReaderAsync();
        await check(reader);
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