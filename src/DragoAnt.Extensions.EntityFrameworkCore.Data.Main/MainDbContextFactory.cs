using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Migrations.Static;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.SqlServer.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.Extensions.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;
using DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main;

// ReSharper disable once UnusedType.Global
public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
{
    public MainDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();
            
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
            
        return new MainDbContext(optionsBuilder.Options);
    }
}