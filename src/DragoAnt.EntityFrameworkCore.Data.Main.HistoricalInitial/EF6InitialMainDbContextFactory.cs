using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalInitial;

// ReSharper disable once UnusedType.Global
public class EF6InitialMainDbContextFactory : IDesignTimeDbContextFactory<HistoricalInitialMainDbContext>
{
    public HistoricalInitialMainDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HistoricalInitialMainDbContext>();
            
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
            
        return new HistoricalInitialMainDbContext(optionsBuilder.Options);
    }
}