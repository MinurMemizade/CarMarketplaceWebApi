using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMarketplaceWebApi.Migrations
{
    /// <inheritdoc />
    public partial class v52 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Seller",
                newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Seller",
                newName: "Phone");
        }
    }
}
