using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DragoAnt.EntityFrameworkCore.DbContext.Initial.Migrations
{
    public partial class ApplyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Iso3LetterCode",
                keyValue: "TS2");

            migrationBuilder.UpdateData(
                table: "Currency",
                keyColumn: "Iso3LetterCode",
                keyValue: "TST",
                columns: new[] { "DecimalDigits", "Description", "Type" },
                values: new object[] { (byte)1, "Test currency Changed", 2 });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Deleted", "Description", "Name", "SourceSystemId" },
                values: new object[,]
                {
                    { new Guid("e017806b-2682-439d-9486-8c13b9f8246a"), null, "Admin desc", "Admin", "e017806b2682439d94868c13b9f8246a" },
                    { new Guid("acadf8c5-dcc8-4f11-aa77-62e2a792f62c"), null, "Customer desc", "Customer", "acadf8c5dcc84f11aa7762e2a792f62c" },
                    { new Guid("2edf50ca-771c-4002-8704-f75e2255d393"), null, "Support desc", "Support", "2edf50ca771c40028704f75e2255d393" }
                });
        }
    }
}
