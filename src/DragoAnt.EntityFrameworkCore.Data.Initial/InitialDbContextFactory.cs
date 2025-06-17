using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.EntityFrameworkCore.Data.Initial.Migrations.Static;
using DragoAnt.EntityFrameworkCore.SqlServer.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.Data.Initial;

// ReSharper disable once UnusedType.Global
public class InitialDbContextFactory : IDesignTimeDbContextFactory<InitialDbContext>
{
    public InitialDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InitialDbContext>();
            
        optionsBuilder.UseSqlServer();
        optionsBuilder.UseStaticMigrationsSqlServer(InitialStaticMigrations.Init);
            
        return new InitialDbContext(optionsBuilder.Options);
    }
}