using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.DbContext.Initial.Migrations
{
    public partial class RoleSourceSystemIdChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Role_SourceSystemId",
                table: "Role");

            migrationBuilder.AlterColumn<string>(
                name: "SourceSystemId",
                table: "Role",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                comment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceSystemId",
                table: "Role",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldComment: "Source system id. Row id for cross services' communication. Uses trigger on row insertion. Configured by convention 'IEntityWithSourceSystemId'");

            migrationBuilder.CreateIndex(
                name: "IX_Role_SourceSystemId",
                table: "Role",
                column: "SourceSystemId",
                unique: true);
        }
    }
}
