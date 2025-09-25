using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace web.apis.Migrations
{
    public partial class secondcommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddedBy = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FullName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ProductCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SoftwareActivationKey = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddedBy = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bac4fac1-c546-41de-aebc-a14da689a0099",
                columns: new[] { "ConcurrencyStamp", "DateAdded", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7076378-e533-495f-aad8-012362b03b34", new DateTime(2025, 9, 25, 13, 17, 40, 198, DateTimeKind.Utc).AddTicks(6362), "AQAAAAEAACcQAAAAEFqJDXaVtiGyhS7NvhwzuVLDY+JcIvugLj7GncMD5a1JnltjKmdc5PWS6AkYjwr+jw==", "322a8a8d-5ebf-475a-b160-4fc66c5cb256" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateAdded",
                value: new DateTime(2025, 9, 25, 13, 17, 40, 200, DateTimeKind.Utc).AddTicks(6246));
        }
    }
}
