using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmerConnectApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class addfarmermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Farmers");
        }
    }
}
