using Microsoft.EntityFrameworkCore.Migrations;

namespace homework.Repository.Migrations
{
    public partial class mvietickt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Movie",
                table: "Tickets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Movie",
                table: "Tickets");
        }
    }
}
