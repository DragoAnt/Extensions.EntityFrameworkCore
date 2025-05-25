using Microsoft.EntityFrameworkCore.Migrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.StaticMigrations.MigrationConditions;

namespace DragoAnt.Extensions.EntityFrameworkCore.DbContext.Initial.Migrations
{
    public partial class Initial : Migration, IWithStaticMigrationActionTag
    {
        public string[] Tags => new string[] { "vCurrencyTag" };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Iso3LetterCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IsoNumericCode = table.Column<int>(type: "int", nullable: false),
                    DecimalDigits = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Iso3LetterCode);
                });
            
            migrationBuilder.CreateTable(
                name: "CurrencyTemp",
                columns: table => new
                {
                    Iso3LetterCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IsoNumericCode = table.Column<int>(type: "int", nullable: false),
                    DecimalDigits = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTemp", x => x.Iso3LetterCode);
                });
            
            migrationBuilder.DropTableIfExists("CurrencyTemp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
