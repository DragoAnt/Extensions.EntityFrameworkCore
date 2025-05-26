using System.ComponentModel.DataAnnotations;

namespace DragoAnt.EntityFrameworkCore.Data.Main;

public enum ContactTypeNullable : byte
{
    [Display(Name = "Person contact nullable")]
    PersonNullable = 1,

    [Display(Name = "Organization contact nullable")]
    OrganizationNullable = 2
}