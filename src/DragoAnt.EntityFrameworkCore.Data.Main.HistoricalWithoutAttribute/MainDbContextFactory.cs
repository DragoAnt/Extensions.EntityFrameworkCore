using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.Migrations.Static;
using DragoAnt.EntityFrameworkCore.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.DependencyInjection;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased;
using DragoAnt.EntityFrameworkCore.EntityConventions.TriggerBased.SqlServer;


namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

// ReSharper disable once UnusedType.Global
public class MainWithoutAttributeDbContextFactory : IDesignTimeDbContextFactory<MainTypeRegistrationDbContext>
{
    public MainTypeRegistrationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MainTypeRegistrationDbContext>();
            
        optionsBuilder.UseSqlServer();
        optionsBuilder.UseEntityConventionsSqlServer(b =>
        {
            b.AddTriggerBasedCommonConventions();
        });
            
        optionsBuilder.UseStaticMigrationsSqlServer(b =>
            {
                MainWithoutAttributeStaticMigrations.Init(b);
                b.AddTriggerBasedEntityConventionsMigrationSqlServer();
            }
        );
            
        return new MainTypeRegistrationDbContext(optionsBuilder.Options);
    }
}