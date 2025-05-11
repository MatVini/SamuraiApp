using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamuraiApp.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Samurai", new[] { "Name", "Clan" }, new object[] { "Takumi", "Dou" });
            migrationBuilder.InsertData("Samurai", new[] { "Name", "Clan" }, new object[] { "Hinata", "Ooboshi" });
            migrationBuilder.InsertData("Samurai", new[] { "Name", "Clan" }, new object[] { "Jousuke", "Sakana" });
            migrationBuilder.InsertData("Samurai", new[] { "Name", "Clan" }, new object[] { "Kamui", "Hanamata" });
            migrationBuilder.InsertData("Samurai", new[] { "Name", "Clan" }, new object[] { "Tomoe", "Dou" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Samurai");
        }
    }
}
