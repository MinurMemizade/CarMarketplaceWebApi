using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMarketplaceWebApi.Migrations
{
    /// <inheritdoc />
    public partial class v73 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_AspNetUsers_AppUserId",
                table: "cars");

            migrationBuilder.DropIndex(
                name: "IX_cars_AppUserId",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "cars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "cars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cars_AppUserId",
                table: "cars",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_AspNetUsers_AppUserId",
                table: "cars",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
