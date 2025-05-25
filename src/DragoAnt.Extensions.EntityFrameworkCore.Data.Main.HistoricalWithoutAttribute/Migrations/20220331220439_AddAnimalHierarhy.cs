using System;
using Microsoft.EntityFrameworkCore.Migrations;
using DragoAnt.Extensions.EntityFrameworkCore.Data.Main;
using DragoAnt.Extensions.EntityFrameworkCore.HistoricalMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.DbContext.Initial.Migrations
{
    public partial class AddAnimalHierarhy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceSystemId",
                table: "Role",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                comment: "Source system id. Row id for cross services' communication. Configured by convention 'IEntityWithSourceSystemId'",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldComment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'");

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

            migrationBuilder.CreateIndex(
                name: "IX_Animal_Discriminator",
                table: "Animal",
                column: "Discriminator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.AlterColumn<string>(
                name: "SourceSystemId",
                table: "Role",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                comment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldComment: "Source system id. Row id for cross services' communication. Configured by convention 'IEntityWithSourceSystemId'");
        }
    }
}
