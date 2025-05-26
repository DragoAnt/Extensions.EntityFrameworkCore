using System.ComponentModel.DataAnnotations;
using DragoAnt.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.EntityFrameworkCore.Data.Initial;

[EnumTable("CurrencyTypes")]
public enum CurrencyType
{
    [Display(Name = "National currency")] 
    National = 1,

    [Display(Name = "Cripto currency")] 
    Cripto = 2
}