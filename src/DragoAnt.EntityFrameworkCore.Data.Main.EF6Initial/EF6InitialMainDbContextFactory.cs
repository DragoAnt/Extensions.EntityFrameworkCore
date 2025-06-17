using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.EntityFrameworkCore.SqlServer.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.Data.Main.EF6Initial;

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