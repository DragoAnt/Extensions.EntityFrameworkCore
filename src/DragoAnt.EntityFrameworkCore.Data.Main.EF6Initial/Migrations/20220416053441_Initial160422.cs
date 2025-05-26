using System;
using Microsoft.EntityFrameworkCore.Migrations;
using DragoAnt.EntityFrameworkCore.Data.Main.EF6Initial;
using DragoAnt.EntityFrameworkCore.HistoricalMigrations;
using DragoAnt.EntityFrameworkCore.HistoricalMigrations.EF6;

namespace DragoAnt.EntityFrameworkCore.Data.Main.HistoricalInitial.Migrations
{
    [EF6InitialMigration(typeof(EF6MigrationManager))]
    public partial class Initial160422 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()", comment: "Row creation datetime. Configured by convention 'ICreateAuditedEntity'"),
                    Discriminator = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, comment: "Discriminator. Configured by convention 'IEntityWithDiscriminator<>'"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()", comment: "Row last modified datetime. Updated by trigger. Configured by convention 'IUpdateAuditedEntity'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeNameNullable = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeName2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeNameNullable = table.Column<byte>(type: "tinyint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()", comment: "Row creation datetime. Configured by convention 'ICreateAuditedEntity'"),
                    SourceSystemId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false, comment: "Source system id. Row id for cross services' communication. Configured by convention 'IEntityWithSourceSystemId'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Iso3LetterCode = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IsoNumericCode = table.Column<int>(type: "int", nullable: false),
                    DecimalDigits = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Iso3LetterCode);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()", comment: "Row creation datetime. Configured by convention 'ICreateAuditedEntity'"),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Row deleted  datetime. Used for soft delete row. Updated by 'instead of' trigger. Configured by convention 'ISoftDeleteEntity'"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Row deleted flag. Used for soft delete row. Updated by 'instead of' trigger. Configured by convention 'ISoftDeleteEntity'"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()", comment: "Row last modified datetime. Updated by trigger. Configured by convention 'IUpdateAuditedEntity'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Iso3LetterCode", "DecimalDigits", "Description", "IsoNumericCode", "Type" },
                values: new object[] { "TST", (byte)1, "Test currency Changed", 1, 2 });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Deleted", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("e017806b-2682-439d-9486-8c13b9f8246a"), null, "Admin desc", "Admin" },
                    { new Guid("acadf8c5-dcc8-4f11-aa77-62e2a792f62c"), null, "Customer desc", "Customer" },
                    { new Guid("2edf50ca-771c-4002-8704-f75e2255d393"), null, "Support desc", "Support" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_Discriminator",
                table: "Animal",
                column: "Discriminator");

            migrationBuilder.CreateIndex(
                name: "IX_Role_IsDeleted",
                table: "Role",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Contact2");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
