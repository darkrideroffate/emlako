using Microsoft.EntityFrameworkCore.Migrations;

namespace Emlakkko.Migrations
{
    public partial class removedevidfromadres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ev_id",
                table: "adres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ev_id",
                table: "adres",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
