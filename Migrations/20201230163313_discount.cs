using Microsoft.EntityFrameworkCore.Migrations;

namespace Emlakkko.Migrations
{
    public partial class discount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "discount",
                table: "ev",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "discountedPrice",
                table: "ev",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount",
                table: "ev");

            migrationBuilder.DropColumn(
                name: "discountedPrice",
                table: "ev");
        }
    }
}
