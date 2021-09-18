using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class Update_Order_Part3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "a0996984-4dc2-41e8-a379-eb424a015151");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "063d82f9-a49e-4ad4-aa1e-78c57c60d956", "AQAAAAEAACcQAAAAEB8ncWn5/PiZ4KUHF0fgZQy1on6qhw0GIyRfXtZU5QkdJMj9wdhGsKOjGrFPjiSnPQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 18, 9, 28, 50, 479, DateTimeKind.Local).AddTicks(5381));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "79f4b1d4-8c5a-4248-90d7-49f98d5f42a4");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bdbb7958-4fec-4242-89fb-bd67ec5a4dfe", "AQAAAAEAACcQAAAAEEe5FutP70QatezcLvYCMcFbWuByy//+T0CxJ1o2TSIRGR9figomRh9GtA4lS5Q9lA==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 9, 17, 14, 16, 19, 86, DateTimeKind.Local).AddTicks(3284));
        }
    }
}
