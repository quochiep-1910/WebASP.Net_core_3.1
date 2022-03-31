using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace eShop.Data.Migrations
{
    public partial class Fix_detail_ProductTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "b384d42b-1c27-4485-ac66-e9e992ecfef6");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "797df239-3dc8-4210-9475-0605e06c9f3b", "AQAAAAEAACcQAAAAEF8T8B11UBVuCwNint/d8vra/AOD1sP8Kk0aKenlBtKZUZoN0jiOGwH/D2vOatYp9Q==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 23, 9, 34, 12, 255, DateTimeKind.Local).AddTicks(6897));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "c5d8802f-ae8c-4b60-aecb-5fc47e0e2d72");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3e3f30a1-2a66-4a28-81bf-eddaa17837f6", "AQAAAAEAACcQAAAAEClwJN9wJuFH3n7Y3e9VEY0e73cuizz1pcs8mq0is4LZ8KP4cU0FkmOYaGac1qkfEQ==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 12, 20, 23, 4, 908, DateTimeKind.Local).AddTicks(5833));
        }
    }
}
