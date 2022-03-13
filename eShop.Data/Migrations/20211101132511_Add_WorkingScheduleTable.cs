using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace eShop.Data.Migrations
{
    public partial class Add_WorkingScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevenueStatistics",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    Revenues = table.Column<decimal>(nullable: true),
                    Benefit = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WorkingSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LyDo = table.Column<string>(maxLength: 200, nullable: false),
                    UserName = table.Column<string>(maxLength: 200, nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingSchedules", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "dad48d64-8f5b-41ef-98b1-7810c8e890d2");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f8161cec-e6cb-4037-a582-9c4e4f33d2d2", "AQAAAAEAACcQAAAAEDzyyBp+OeYF5G3gmdOkB4ZPz1esDf6VYOVMf8DrHLyNeSPNgtIwuS2JaOkh2ewyDg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 11, 1, 20, 25, 5, 489, DateTimeKind.Local).AddTicks(2528));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueStatistics");

            migrationBuilder.DropTable(
                name: "WorkingSchedules");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "eae23a75-56ee-4d0f-bc25-b0a4cf377ccb");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "019a9371-18ef-45d1-aac9-fe24699a6158", "AQAAAAEAACcQAAAAEB96AoIHrZV0n/1VoCePlHRa2XWx2pas4OslpSYJSeoDLL8HK8Ns/UWFMfoFtB2FkQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 19, 9, 18, 47, 40, DateTimeKind.Local).AddTicks(6414));
        }
    }
}
