using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Initial.Migrations.Static;

public static class InitialStaticMigrations
{
    public static void Init(StaticMigrationBuilder migrations)
    {
        migrations.AddInitialSqlResFile("InitDB", suppressTransaction: true);

        migrations.AddSqlResFile("TestViews");
        migrations.AddSqlResFile("vCurrency");
    }
}