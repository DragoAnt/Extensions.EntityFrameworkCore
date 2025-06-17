using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;

namespace DragoAnt.EntityFrameworkCore.Data.Main;

// ReSharper disable once UnusedType.Global
public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();

        optionsBuilder.UseSqlServer();
        optionsBuilder.UseEntityConventionsSqlServer(b => { b.AddTriggerBasedCommonConventions(); });

        optionsBuilder.UseStaticMigrationsSqlServer(b =>
            {
                MainStaticMigrations.Init(b);
                b.AddTriggerBasedEntityConventionsMigrationSqlServer();
            }
        );

        return new MainDbContext(optionsBuilder.Options);
    }
}