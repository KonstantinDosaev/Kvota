using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    public partial class DateAndQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateDelivery",
                table: "Products",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityTwo",
                table: "Products",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDelivery",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityTwo",
                table: "Products");
        }
    }
}
