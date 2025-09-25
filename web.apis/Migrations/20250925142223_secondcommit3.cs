using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.apis.Migrations
{
    public partial class secondcommit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1e30a67-b7d0-4f74-bb0b-f78bf407129e", new DateTime(2025, 9, 25, 14, 22, 22, 702, DateTimeKind.Utc).AddTicks(4507), "AQAAAAEAACcQAAAAEMl4jHJKbwgEWkq175Ax5ssiIP4OO3mmukQ6IWjxB5dViFD/Sa9nfDPQDtSrFaM69w==", "db2aaa99-89df-4c26-8ebb-323e4dd21b00" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 14, 22, 22, 703, DateTimeKind.Utc).AddTicks(9885));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f62dc3f-4850-4903-b42b-843847761bbc", new DateTime(2025, 9, 25, 14, 20, 19, 241, DateTimeKind.Utc).AddTicks(7776), "AQAAAAEAACcQAAAAEM1fwlElxonjPh1YWBE3zRI827nXJGdS3FMVm3wMEs18bnsEgQiNJ9AtqUECkwEWdg==", "0e8cec06-2c78-4dfd-ba3b-3ea0f4fe240a" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 14, 20, 19, 243, DateTimeKind.Utc).AddTicks(1927));
        }
    }
}
