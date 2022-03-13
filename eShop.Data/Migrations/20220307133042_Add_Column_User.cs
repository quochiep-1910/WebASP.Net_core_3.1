using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class Add_Column_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "AppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordReset",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "418e9c8a-45dd-4599-9f35-18d537131afc");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2b9658f8-b769-4481-9b2e-910de97b6800", "AQAAAAEAACcQAAAAENcClRgcGxeDToPwKMElMToCvcfU+lkFLDXs8DaJtzX2KjRXvwCW99LpZyqTb2iEUg==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 3, 7, 20, 30, 41, 837, DateTimeKind.Local).AddTicks(8175));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PasswordReset",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "AppUsers");

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
    }
}
