using DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.Migrations.Static;

public static class MainStaticMigrations
{
    public static void Init(StaticMigrationBuilder migrations)
    {
        migrations.AddInitialSqlResFile("InitDB", suppressTransaction: true);

        migrations.AddSqlResFile("TestViews");
        migrations.AddSqlResFile("vCurrency");
            
        migrations.AddReportingFile("_ReportingSchema", ResSqlFile.Apply);
        migrations.AddReportingFile("TestReporting");
    }
}

internal static class MainStaticMigrationsExtensions
{
    public static void AddReportingFile(this StaticMigrationBuilder migrations, string mainFileName, ResSqlFile type = ResSqlFile.All)
    {
        migrations.AddSqlResFile($@"_Reporting\{mainFileName}", type);
    } 
}