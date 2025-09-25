using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace web.apis.Migrations
{
    public partial class secondcommit1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoftwareActivationKey = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ProductCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Link = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DateAdded = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AddedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fea74245-d3aa-4953-95c0-9cc90822f923", new DateTime(2025, 9, 25, 14, 7, 10, 684, DateTimeKind.Utc).AddTicks(6713), "AQAAAAEAACcQAAAAEOv8AmnWJqt0+x8z1GYL5ynMnJ1YrbGcHBn5I5Ea/XzPpF4PAlOCVVFsnXs82q7TUg==", "8e0543da-9d29-4d29-85a6-fcf437059527" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 14, 7, 10, 686, DateTimeKind.Utc).AddTicks(410));
        }
    }
}
