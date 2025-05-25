using System.ComponentModel.DataAnnotations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public enum ContactType : byte
{
    [Display(Name = "Person contact")] 
    Person = 1,

    [Display(Name = "Organization contact")]
    Organization = 2
}