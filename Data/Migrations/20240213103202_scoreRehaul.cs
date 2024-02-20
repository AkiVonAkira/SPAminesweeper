using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class scoreRehaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "GameModel");

            migrationBuilder.AddColumn<int>(
                name: "ScoreId",
                table: "GameModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_ScoreId",
                table: "GameModel",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameModel_ScoreModel_ScoreId",
                table: "GameModel",
                column: "ScoreId",
                principalTable: "ScoreModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameModel_ScoreModel_ScoreId",
                table: "GameModel");

            migrationBuilder.DropIndex(
                name: "IX_GameModel_ScoreId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "GameModel");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "GameModel",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
