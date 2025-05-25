using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.Tests.HistoricalMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests;

public class HistoricalMigrationsTests
{
    [Fact]
    public void CombineHistorical0_Empty()
    {
        TestHistorical<DbContext0>(ArraySegment<string>.Empty,
            ["00_StartInitialMigration", "01_Migration01"]);
    }

    [Fact]
    public void CombineHistorical0_00Applied()
    {
        TestHistorical<DbContext0>(["00_StartInitialMigration"],
            ["01_Migration01"]);
    }

    [Fact]
    public void CombineHistorical1_Empty()
    {
        TestHistorical<DbContext1>(ArraySegment<string>.Empty,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);
    }

    [Fact]
    public void CombineHistorical1_AppliedTo_00_StartInitialMigration()
    {
        TestHistorical<DbContext1>(["00_StartInitialMigration"],
        [
            "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);
    }

    [Fact]
    public void CombineHistorical1_AppliedTo_11_Migration11()
    {
        TestHistorical<DbContext1>([
                "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11",
            ],
            ["12_Migration12"]);
    }

    [Fact]
    public void CombineHistorical2_Empty()
    {
        var actual = TestHistorical<DbContext2>(ArraySegment<string>.Empty,
        [
            "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);

        actual[0].Value.Should().Be<HistoricalInitialMigration20>();
    }

    [Fact]
    public void CombineHistorical2_AppliedTo_00_StartInitialMigration()
    {
        var actual = TestHistorical<DbContext2>(["00_StartInitialMigration"],
        [
            "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
                "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);

        TestInitialMigrationReplace(actual, 4,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);
    }

    [Fact]
    public void CombineHistorical2_AppliedTo_11_Migration11()
    {
        var actual = TestHistorical<DbContext2>([
                "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11",
            ],
            [
                "12_Migration12",
                "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
            ]);


        TestInitialMigrationReplace(actual, 1,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);
    }

    [Fact]
    public void CombineHistorical2_AppliedTo_20_HistoricalInitialMigration20()
    {
        TestHistorical<DbContext2>(["20_HistoricalInitialMigration20"],
            ["21_Migration21", "22_Migration22"]);
    }

    [Fact]
    public void CombineHistorical2_AppliedTo_21_Migration21()
    {
        TestHistorical<DbContext2>(["20_HistoricalInitialMigration20", "21_Migration21"],
            ["22_Migration22"]);
    }

    [Fact]
    public void CombineHistorical3_Empty()
    {
        var actual = TestHistorical<DbContext3>(ArraySegment<string>.Empty,
        [
            "31_HistoricalInitialMigration31", "30_Migration30", "32_Migration32",
        ]);

        actual[0].Value.Should().Be<HistoricalInitialMigration31>();
    }

    [Fact]
    public void CombineHistorical3_AppliedTo_00_StartInitialMigration()
    {
        var actual = TestHistorical<DbContext3>(["00_StartInitialMigration"],
        [
            "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
                "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
                "31_HistoricalInitialMigration31", "30_Migration30", "32_Migration32",
        ]);

        TestInitialMigrationReplace(actual, 4,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);

        TestInitialMigrationReplace(actual, 7,
        [
            "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);
    }

    [Fact]
    public void CombineHistorical3_AppliedTo_11_Migration11()
    {
        var actual = TestHistorical<DbContext3>([
                "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11",
            ],
            [
                "12_Migration12",
                "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
                "31_HistoricalInitialMigration31", "30_Migration30", "32_Migration32",
            ]);

        TestInitialMigrationReplace(actual, 1,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);

        TestInitialMigrationReplace(actual, 4,
        [
            "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);
    }

    [Fact]
    public void CombineHistorical3_AppliedTo_21_Migration21()
    {
        var actual = TestHistorical<DbContext3>([
                "20_HistoricalInitialMigration20", "21_Migration21",
            ],
            [
                "22_Migration22",
                "31_HistoricalInitialMigration31", "30_Migration30", "32_Migration32",
            ]);

        TestInitialMigrationReplace(actual, 1,
        [
            "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);
    }


    [Fact]
    public void CombineHistorical3_AppliedTo_HistoricalInitialMigration31()
    {
        TestHistorical<DbContext3>(["31_HistoricalInitialMigration31"],
            ["30_Migration30", "32_Migration32"]);
    }

    [Fact]
    public void CombineHistorical3_AppliedTo_30_Migration30()
    {
        TestHistorical<DbContext3>(["31_HistoricalInitialMigration31", "30_Migration30"],
            ["32_Migration32"]);
    }
        
    [Fact]
    public void CombineHistorical3_Empty_FullHistory()
    {
        TestHistorical<DbContext3>([],
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
                "21_Migration21", "22_Migration22",
                "30_Migration30", "32_Migration32",
        ], true);
            
        //NOTE: Test run after run with full history enabled
        var actual = TestHistorical<DbContext3>([
                "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
                "21_Migration21", "22_Migration22",
                "30_Migration30", "32_Migration32",
            ],
            [
                "20_HistoricalInitialMigration20", "31_HistoricalInitialMigration31",
            ]);
            
        TestInitialMigrationReplace(actual, 0,
        [
            "00_StartInitialMigration", "01_Migration01",
                "10_HistoricalMigration10", "11_Migration11", "12_Migration12",
        ]);
        TestInitialMigrationReplace(actual, 1,
        [
            "20_HistoricalInitialMigration20", "21_Migration21", "22_Migration22",
        ]);
    }

    private List<KeyValuePair<string, TypeInfo>> TestHistorical<TDbContext>(IEnumerable<string> appliedMigrationEntries, string[] expected,
        bool migrateFromFullHistory = false)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var historicalMigrations = GetHistoricalMigrations<TDbContext>(options => options.MigrateFromFullHistory = migrateFromFullHistory);

        var actual = historicalMigrations.PopulateMigrations(appliedMigrationEntries).ToList();
        var actualIds = actual.Select(m => m.Key).ToList();

        actualIds.Should().ContainInOrder(expected);

        return actual;
    }

    private static void TestInitialMigrationReplace(IReadOnlyList<KeyValuePair<string, TypeInfo>> actual, 
        int index, string[] expectedRemoveIds)
    {
        actual[index].Value.Should().BeAssignableTo<InitialReplaceMigration>();
        actual[index].Value.GetInitialMigration()
            .RemoveMigrationRowIds.Should().ContainInOrder(expectedRemoveIds);
    }

    private static string GetConnectionString(string dbName)
    {
        return $@"Data Source=.\SQLEXPRESS;Initial Catalog={dbName};Integrated Security=SSPI";
    }

    private static TDbContext GetContext<TDbContext>(Action<HistoricalMigrationsOptions> init)
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var services = new ServiceCollection();

        var connectionString = GetConnectionString("test_Historical");

        services.AddDbContext<TDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
                builder.UseHistoricalMigrations(init);
            },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<TDbContext>();
    }

    private static HistoricalMigrationsAssembly GetHistoricalMigrations<TContext>(Action<HistoricalMigrationsOptions> init)
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        var context = GetContext<TContext>(init);
        return (HistoricalMigrationsAssembly)context.GetInfrastructure().GetRequiredService<IMigrationsAssembly>();
    }
}