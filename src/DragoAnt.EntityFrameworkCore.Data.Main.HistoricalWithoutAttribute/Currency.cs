namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public class Currency
{
    public static Currency Create(int isoNumericCode, string iso3LetterCode,  byte decimalDigits, string description, CurrencyType currencyType)
    {
        return new Currency(iso3LetterCode, isoNumericCode, decimalDigits, description, currencyType);
    }
        
    private Currency(string iso3LetterCode, int isoNumericCode, byte decimalDigits, string description, CurrencyType type)
    {
        Iso3LetterCode = iso3LetterCode;
        IsoNumericCode = isoNumericCode;
        DecimalDigits = decimalDigits;
        Description = description;
        Type = type;
    }

    public string Iso3LetterCode { get; private set; }
    public int IsoNumericCode { get; private set; }
    public byte DecimalDigits { get; init; }
    public string Description { get; protected set; }
    public CurrencyType Type { get; set; }
}