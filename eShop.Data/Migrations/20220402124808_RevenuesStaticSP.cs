using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace eShop.Data.Migrations
{
    public partial class RevenuesStaticSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: "69BD714F-9576-45BA-B5B7-F00649BE00DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0765edb7-474e-4086-abcb-59b93b1e133f", "AQAAAAEAACcQAAAAEAIXs1lXRCwyJwOlnMAUvXe+nY2aNtG7EjxlaBmdtrTT9oT6CYK+mDLr/ovuKPFzuQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8D04DCE2-969A-435D-BBA4-DF3F325983DC",
                column: "ConcurrencyStamp",
                value: "6331a69a-cf72-4e46-bf07-99cfc83fb383");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 4, 2, 19, 48, 7, 713, DateTimeKind.Local).AddTicks(7695));
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
                        where o.OrderDate <= cast(@toDate as date) and o.OrderDate >= cast(@fromDate as date) and o.Status = 3
                        group by o.OrderDate
                   END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}