using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class scoreGameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "ScoreModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreModel_GameId",
                table: "ScoreModel",
                column: "GameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreModel_GameModel_GameId",
                table: "ScoreModel",
                column: "GameId",
                principalTable: "GameModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreModel_GameModel_GameId",
                table: "ScoreModel");

            migrationBuilder.DropIndex(
                name: "IX_ScoreModel_GameId",
                table: "ScoreModel");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "ScoreModel");

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
    }
}
