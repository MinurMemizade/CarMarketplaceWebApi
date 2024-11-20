using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMarketplaceWebApi.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_SellerId",
                table: "cars",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_Seller_SellerId",
                table: "cars",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_Seller_SellerId",
                table: "cars");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.DropIndex(
                name: "IX_cars_SellerId",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "cars");
        }
    }
}
