using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMarketplaceWebApi.Migrations
{
    /// <inheritdoc />
    public partial class v74 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_cars_UserId",
                table: "cars",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_AspNetUsers_UserId",
                table: "cars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_AspNetUsers_UserId",
                table: "cars");

            migrationBuilder.DropIndex(
                name: "IX_cars_UserId",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "cars");
        }
    }
}
