using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvota.Migrations
{
    public partial class Content : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressOne = table.Column<string>(type: "text", nullable: true),
                    AddressTwo = table.Column<string>(type: "text", nullable: true),
                    PhoneOne = table.Column<string>(type: "text", nullable: true),
                    PhoneTwo = table.Column<string>(type: "text", nullable: true),
                    PhoneThree = table.Column<string>(type: "text", nullable: true),
                    EmailOne = table.Column<string>(type: "text", nullable: true),
                    EmailTwo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeImageTextOne = table.Column<string>(type: "text", nullable: true),
                    HomeImageTextTwo = table.Column<string>(type: "text", nullable: true),
                    PartnerTitle = table.Column<string>(type: "text", nullable: true),
                    AboutHomeText = table.Column<string>(type: "text", nullable: true),
                    AboutText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Homes");
        }
    }
}
