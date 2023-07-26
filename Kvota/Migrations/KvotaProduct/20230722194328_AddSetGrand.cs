using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    public partial class AddSetGrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_GrandCategory_GrandCategoryId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GrandCategory",
                table: "GrandCategory");

            migrationBuilder.RenameTable(
                name: "GrandCategory",
                newName: "GrandCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GrandCategories",
                table: "GrandCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_GrandCategories_GrandCategoryId",
                table: "Categories",
                column: "GrandCategoryId",
                principalTable: "GrandCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_GrandCategories_GrandCategoryId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GrandCategories",
                table: "GrandCategories");

            migrationBuilder.RenameTable(
                name: "GrandCategories",
                newName: "GrandCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GrandCategory",
                table: "GrandCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_GrandCategory_GrandCategoryId",
                table: "Categories",
                column: "GrandCategoryId",
                principalTable: "GrandCategory",
                principalColumn: "Id");
        }
    }
}
