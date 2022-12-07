using Microsoft.EntityFrameworkCore.Migrations;

namespace Pustok.Migrations
{
    public partial class SlidersTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title1 = table.Column<string>(maxLength: 30, nullable: true),
                    Title2 = table.Column<string>(maxLength: 30, nullable: true),
                    Desc = table.Column<string>(maxLength: 150, nullable: true),
                    Image = table.Column<string>(maxLength: 200, nullable: true),
                    BtnText = table.Column<string>(maxLength: 30, nullable: true),
                    RedirectedUrl = table.Column<string>(maxLength: 100, nullable: true),
                    Order = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sliders");
        }
    }
}
