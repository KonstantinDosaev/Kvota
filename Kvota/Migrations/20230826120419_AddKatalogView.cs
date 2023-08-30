using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations
{
    public partial class AddKatalogView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CatalogView",
                table: "Homes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatalogView",
                table: "Homes");
        }
    }
}
