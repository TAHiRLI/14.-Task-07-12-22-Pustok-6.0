using Microsoft.EntityFrameworkCore.Migrations;

namespace Pustok.Migrations
{
    public partial class BookidSolving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_Books_BooksId",
                table: "BookImages");

            migrationBuilder.DropIndex(
                name: "IX_BookImages_BooksId",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "BooksId",
                table: "BookImages");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_BookId",
                table: "BookImages",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages");

            migrationBuilder.DropIndex(
                name: "IX_BookImages_BookId",
                table: "BookImages");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookImages");

            migrationBuilder.AddColumn<int>(
                name: "BooksId",
                table: "BookImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_BooksId",
                table: "BookImages",
                column: "BooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_Books_BooksId",
                table: "BookImages",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
