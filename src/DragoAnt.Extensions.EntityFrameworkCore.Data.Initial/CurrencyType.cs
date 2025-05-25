using System.ComponentModel.DataAnnotations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.Enums;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Initial;

[EnumTable("CurrencyTypes")]
public enum CurrencyType
{
    [Display(Name = "National currency")] 
    National = 1,

    [Display(Name = "Cripto currency")] 
    Cripto = 2
}