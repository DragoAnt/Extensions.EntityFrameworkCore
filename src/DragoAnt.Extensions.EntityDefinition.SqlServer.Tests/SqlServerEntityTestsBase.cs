using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityDefinition.Model;

namespace DragoAnt.Extensions.EntityDefinition.SqlServer.Tests;

public abstract class SqlServerEntityTestsBase
{
    protected SqlServerEntityTestsBase()
    {
        ServiceProvider = GetServices<DefinitionDbContext>();
        DbContext = ServiceProvider.GetRequiredService<DefinitionDbContext>();
    }

    protected readonly DefinitionDbContext DbContext;
    protected readonly IServiceProvider ServiceProvider;

    private const string DbName = "stenn_definitions_efcore_tests";

    protected static string GetConnectionString(string dbName)
    {
        return $@"Data Source=.\SQLEXPRESS;Initial Catalog={dbName};Integrated Security=SSPI";
    }


    private static IServiceProvider GetServices<TDbContext>()
        where TDbContext : DbContext
    {
        var services = new ServiceCollection();

        var connectionString = GetConnectionString(DbName);

        services.AddDbContext<TDbContext>(builder => { builder.UseSqlServer(connectionString); },
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services.BuildServiceProvider();
    }
}