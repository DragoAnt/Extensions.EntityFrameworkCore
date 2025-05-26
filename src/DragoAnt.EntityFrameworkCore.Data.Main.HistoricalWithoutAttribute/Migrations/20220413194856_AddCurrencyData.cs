using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.DbContext.Main.Migrations
{
    public partial class AddCurrencyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Iso3LetterCode", "DecimalDigits", "Description", "IsoNumericCode", "Type" },
                values: new object[] { "TST", (byte)2, "Test currency", 1, 1 });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Iso3LetterCode", "DecimalDigits", "Description", "IsoNumericCode", "Type" },
                values: new object[] { "TS2", (byte)2, "Test currency 2", 2, 1 });
        }
    }
}
