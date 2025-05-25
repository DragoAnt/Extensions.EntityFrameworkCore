namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.HistoricalWithoutAttribute.StaticMigrations.DictEntities;

public static class RoleDeclaration
{
    public static List<Role> GetActual()
    {
        return
        [
            Role.Create(Roles.Admin, "Admin", "Admin desc"),
            Role.Create(Roles.Customer, "Customer", "Customer desc"),
            Role.Create(Roles.Support, "Support", "Support desc"),
        ];
    }
}