using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShop.Data.Migrations
{
    public partial class RevenuesStaticSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            ///Create Store
            var sp = @"Create PROCEDURE [dbo].[GetRevenueStatistic]
                         @fromDate [nvarchar](max),
                         @toDate [nvarchar](max)
                    AS
                    BEGIN
                        select
                        o.OrderDate as Date,
                        sum(od.Quantity * od.Price) as Revenues,
                        sum((od.Quantity * od.Price)-(od.Quantity*p.OriginalPrice)) as Benefit
                        from Orders o
                        inner join OrderDetails od
                        on o.ID = od.OrderID
                        inner join Products p
                        on od.ProductID =p.ID
                        where o.OrderDate <= cast(@toDate as date) and o.OrderDate >= cast(@fromDate as date)
                        group by o.OrderDate
                   END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}