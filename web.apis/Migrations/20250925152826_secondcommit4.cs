using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.apis.Migrations
{
    public partial class secondcommit4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoftwareActivationKey",
                table: "Modules",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Modules",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Modules",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Modules",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a98aa92b-f0a9-4ace-acdd-3fa7737a014d", new DateTime(2025, 9, 25, 15, 28, 26, 403, DateTimeKind.Utc).AddTicks(1117), "AQAAAAEAACcQAAAAEMeh4emuZS+ZdHwTYWcB8gAWiSEAdGH72rZT0o/ePRpOO1B1doZvhuzJnvg//v0P1A==", "5a8735b0-8b2c-4fe8-b898-910c6f3e65ac" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 15, 28, 26, 404, DateTimeKind.Utc).AddTicks(8564));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoftwareActivationKey",
                table: "Modules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                table: "Modules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Modules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Modules",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

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
    }
}
