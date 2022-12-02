using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Services.Migrations
{
    public partial class addRatetoBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RateCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RateValue",
                table: "Books",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RateValue",
                table: "Books");
        }
    }
}
