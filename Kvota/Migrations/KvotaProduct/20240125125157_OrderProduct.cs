﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations.KvotaProduct
{
    public partial class OrderProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationOrderingProductsProduct");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MissingProductsInCatalog",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ApplicationOrderingProducts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationOrderingProductsProducts",
                columns: table => new
                {
                    ApplicationOrderingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationOrderingProductsProducts", x => new { x.ApplicationOrderingId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProducts_ApplicationOrderingProd~",
                        column: x => x.ApplicationOrderingId,
                        principalTable: "ApplicationOrderingProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOrderingProductsProducts_ProductId",
                table: "ApplicationOrderingProductsProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationOrderingProductsProducts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "MissingProductsInCatalog",
                table: "ApplicationOrderingProducts");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ApplicationOrderingProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ApplicationOrderingProducts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ApplicationOrderingProductsProduct",
                columns: table => new
                {
                    ApplicationOrderingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductListId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationOrderingProductsProduct", x => new { x.ApplicationOrderingListId, x.ProductListId });
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProduct_ApplicationOrderingProdu~",
                        column: x => x.ApplicationOrderingListId,
                        principalTable: "ApplicationOrderingProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationOrderingProductsProduct_Products_ProductListId",
                        column: x => x.ProductListId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOrderingProductsProduct_ProductListId",
                table: "ApplicationOrderingProductsProduct",
                column: "ProductListId");
        }
    }
}
