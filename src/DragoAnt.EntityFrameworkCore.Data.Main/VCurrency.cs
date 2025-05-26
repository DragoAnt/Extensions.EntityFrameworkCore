namespace DragoAnt.EntityFrameworkCore.Data.Main;

public class VCurrency
{
    private VCurrency(string iso3LetterCode, CurrencyType type)
    {
        Iso3LetterCode = iso3LetterCode;
        Type = type;
    }

    public string Iso3LetterCode { get; private set; }
    public CurrencyType Type { get; set; }
}