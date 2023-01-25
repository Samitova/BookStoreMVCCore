using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Services.Migrations
{
    public partial class constraintForComments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_Books_BookId",
                table: "BookComments");

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_Books_BookId",
                table: "BookComments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookComments_Books_BookId",
                table: "BookComments");

            migrationBuilder.AddForeignKey(
                name: "FK_BookComments_Books_BookId",
                table: "BookComments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
