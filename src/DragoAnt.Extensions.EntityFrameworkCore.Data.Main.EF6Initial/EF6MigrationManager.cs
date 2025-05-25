using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations.EF6;

namespace DragoAnt.Extensions.EntityFrameworkCore.Data.Main.EF6Initial;

internal sealed class EF6MigrationManager : EF6MigrationManagerBase
{
    /// <inheritdoc />
    public override string[] MigrationIds =>
    [
        "20220122095651_Initial",
        "20220327210310_AddRole_Contact",
        "20220328093627_RoleSoftDelete",
        "20220328125549_RoleSoftDelete2",
        "20220329214635_RoleSourceSystemIdChange",
        "20220329215435_AddCurrencyType",
        "20220331220439_AddAnimalHierarhy",
        "20220413194856_AddCurrencyData",
        "20220413201748_ApplyData",
        "20220414150604_AddContactNullableEnumColumnAndContact2",
    ];
}