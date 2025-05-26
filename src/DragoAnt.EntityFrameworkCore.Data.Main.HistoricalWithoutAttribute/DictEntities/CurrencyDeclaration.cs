namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.StaticMigrations.DictEntities;

public static class CurrencyDeclaration
{
    public static List<Currency> GetActual()
    {
        return [Currency.Create(1, "TST", 1, "Test currency Changed", CurrencyType.Cripto)];
    }
}