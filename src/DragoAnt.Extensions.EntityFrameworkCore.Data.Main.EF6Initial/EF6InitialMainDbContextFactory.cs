using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.SqlServer.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.EF6Initial;

// ReSharper disable once UnusedType.Global
public class HistoricalInitialMainDbContextFactory : IDesignTimeDbContextFactory<EF6InitialMainDbContext>
{
    public EF6InitialMainDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EF6InitialMainDbContext>();
            
        optionsBuilder.UseSqlServer();
        optionsBuilder.UseEntityConventionsSqlServer(b =>
        {
            b.AddTriggerBasedCommonConventions();
        });
            
        optionsBuilder.UseStaticMigrationsSqlServer(b =>
            {
                MainStaticMigrations.Init(b);
                b.AddTriggerBasedEntityConventionsMigrationSqlServer();
            }
        );
            
        return new EF6InitialMainDbContext(optionsBuilder.Options);
    }
}