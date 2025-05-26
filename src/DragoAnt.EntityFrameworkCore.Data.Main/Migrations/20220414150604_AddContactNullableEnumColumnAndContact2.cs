using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.DbContext.Initial.Migrations
{
    public partial class AddContactNullableEnumColumnAndContact2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceSystemId",
                table: "Role");

            migrationBuilder.AddColumn<byte>(
                name: "TypeNameNullable",
                table: "Contact",
                type: "tinyint",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact2");

            migrationBuilder.DropColumn(
                name: "TypeNameNullable",
                table: "Contact");

            migrationBuilder.AddColumn<string>(
                name: "SourceSystemId",
                table: "Role",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Source system id. Row id for cross services' communication. Configured by convention 'IEntityWithSourceSystemId'");
        }
    }
}
