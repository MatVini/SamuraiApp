using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamuraiApp.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class NtoN_relation_SamKen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kenjutsu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kenjutsu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KenjutsuSamurai",
                columns: table => new
                {
                    KenCollectId = table.Column<int>(type: "int", nullable: false),
                    SamCollectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KenjutsuSamurai", x => new { x.KenCollectId, x.SamCollectId });
                    table.ForeignKey(
                        name: "FK_KenjutsuSamurai_Kenjutsu_KenCollectId",
                        column: x => x.KenCollectId,
                        principalTable: "Kenjutsu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KenjutsuSamurai_Samurai_SamCollectId",
                        column: x => x.SamCollectId,
                        principalTable: "Samurai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KenjutsuSamurai_SamCollectId",
                table: "KenjutsuSamurai",
                column: "SamCollectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KenjutsuSamurai");

            migrationBuilder.DropTable(
                name: "Kenjutsu");
        }
    }
}
