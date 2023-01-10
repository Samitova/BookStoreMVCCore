using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Services.Migrations
{
    public partial class ratingChanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RateValue",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RateCount",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "RateValue",
                table: "Authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RateCount",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RateValue",
                table: "Authors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
