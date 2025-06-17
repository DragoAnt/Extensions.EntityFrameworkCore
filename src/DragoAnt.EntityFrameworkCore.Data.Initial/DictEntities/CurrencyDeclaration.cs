namespace DragoAnt.EntityFrameworkCore.Data.Initial.DictEntities;

public static class CurrencyDeclaration
{
    public static List<CurrencyV1> GetActual()
    {
        return
        [
            CurrencyV1.Create(1, "TST", 2, "Test currency", CurrencyType.National), CurrencyV1.Create(2, "TS2", 2, "Test currency 2", CurrencyType.National),
        ];
    }
}