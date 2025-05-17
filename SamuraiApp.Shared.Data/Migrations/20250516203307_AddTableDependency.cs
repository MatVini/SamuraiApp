using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamuraiApp.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DojoId",
                table: "Samurai",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Samurai_DojoId",
                table: "Samurai",
                column: "DojoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Samurai_Dojo_DojoId",
                table: "Samurai",
                column: "DojoId",
                principalTable: "Dojo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Samurai_Dojo_DojoId",
                table: "Samurai");

            migrationBuilder.DropIndex(
                name: "IX_Samurai_DojoId",
                table: "Samurai");

            migrationBuilder.DropColumn(
                name: "DojoId",
                table: "Samurai");
        }
    }
}
