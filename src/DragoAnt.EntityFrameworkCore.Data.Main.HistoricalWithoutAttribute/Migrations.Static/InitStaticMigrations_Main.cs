using DragoAnt.EntityFrameworkCore.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.Migrations.Static;

public static class MainWithoutAttributeStaticMigrations
{
    public static void Init(StaticMigrationBuilder migrations)
    {
        migrations.AddInitialSqlResFile("InitDB", suppressTransaction: true);

        migrations.AddSqlResFile("TestViews");
        migrations.AddSqlResFile("vCurrency");
    }
}