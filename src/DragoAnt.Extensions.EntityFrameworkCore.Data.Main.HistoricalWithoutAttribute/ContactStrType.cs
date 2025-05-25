using System.ComponentModel.DataAnnotations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute;

public enum ContactStrType : byte
{
    [Display(Name = "Person str contact")] 
    Person2 = 1,

    [Display(Name = "Organization str contact")]
    Organization2 = 2
}