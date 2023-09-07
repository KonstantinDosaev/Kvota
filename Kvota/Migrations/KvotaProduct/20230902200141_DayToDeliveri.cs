using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    public partial class DayToDeliveri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayToDelivery",
                table: "Products",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayToDelivery",
                table: "Products");
        }
    }
}
