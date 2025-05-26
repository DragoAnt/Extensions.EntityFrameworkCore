namespace DragoAnt.EntityDefinition.Model;

public class Money
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; } = new();
}

public class Currency
{
    public int IsoNumericCode { get; set; }
}