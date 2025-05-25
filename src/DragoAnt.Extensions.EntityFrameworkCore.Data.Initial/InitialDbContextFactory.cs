using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Initial.Migrations.Static;
using DragoAnt.Extensions.EntityFrameworkCore.SqlServer.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;

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