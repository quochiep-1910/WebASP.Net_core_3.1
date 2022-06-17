using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class updateidentityv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "69BD714F-9576-45BA-B5B7-F00649BE00DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b76eb5ed-86b3-4ef0-ad83-4bf453c7c1d1", "AQAAAAEAACcQAAAAENdo4fbpzNxfoW73O9N9VphKwSjIvhWtRoX2OIjHrthStCqtdX6mCpIVkJ0VbzIGuQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8D04DCE2-969A-435D-BBA4-DF3F325983DC",
                column: "ConcurrencyStamp",
                value: "a3644d56-77fa-4a1b-ba64-a5220ce2b820");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 3, 31, 20, 30, 29, 2, DateTimeKind.Local).AddTicks(293));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "69BD714F-9576-45BA-B5B7-F00649BE00DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5e6c859-07b0-44ce-b486-d71b54d29ce2", "AQAAAAEAACcQAAAAEF5p56xIPuDYHoAQujM0CbLRzYXAi9H2oNkrzqOJE+Bb1dTQvXArfQTPkBAi2SxlTw==" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8D04DCE2-969A-435D-BBA4-DF3F325983DC",
                column: "ConcurrencyStamp",
                value: "63adac6f-9805-4973-b285-b4407e45089f");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 3, 31, 20, 9, 43, 383, DateTimeKind.Local).AddTicks(3708));
        }
    }
}
