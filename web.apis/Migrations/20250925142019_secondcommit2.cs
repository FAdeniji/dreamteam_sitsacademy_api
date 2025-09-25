using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.apis.Migrations
{
    public partial class secondcommit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoftwareActivationKey",
                table: "Course",
                newName: "ShortName");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Course",
                newName: "SoftwareActivationKey");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86645425-10c6-4084-b54d-906da492ebdb", new DateTime(2025, 9, 25, 14, 11, 46, 117, DateTimeKind.Utc).AddTicks(6580), "AQAAAAEAACcQAAAAEMQZ0Ov/BBefv9sMPLsgDlyY/7vhJo8uCXw8XtaLKgZjZISdNCLgNzdi4IRKGNQ2Tw==", "6f4ec4a2-823f-4a98-8bc4-82826dc4b8f8" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 14, 11, 46, 119, DateTimeKind.Utc).AddTicks(1709));
        }
    }
}
