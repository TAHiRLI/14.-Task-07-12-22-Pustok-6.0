using Microsoft.EntityFrameworkCore.Migrations;

namespace Pustok.Migrations
{
    public partial class BtmPromotionsTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BtmPromotions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title1 = table.Column<string>(maxLength: 80, nullable: true),
                    Title2 = table.Column<string>(maxLength: 80, nullable: true),
                    Title3 = table.Column<string>(maxLength: 80, nullable: true),
                    BtnText = table.Column<string>(maxLength: 80, nullable: true),
                    RedirectedUrl = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BtmPromotions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BtmPromotions");
        }
    }
}
