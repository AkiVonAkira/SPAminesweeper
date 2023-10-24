using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Migrations
{
    /// <inheritdoc />
    public partial class brah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardModel_GameModel_GameId",
                table: "BoardModel");

            migrationBuilder.DropIndex(
                name: "IX_BoardModel_GameId",
                table: "BoardModel");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "BoardModel");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "GameModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_BoardId",
                table: "GameModel",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameModel_BoardModel_BoardId",
                table: "GameModel",
                column: "BoardId",
                principalTable: "BoardModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameModel_BoardModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropIndex(
                name: "IX_GameModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "GameModel");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "BoardModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BoardModel_GameId",
                table: "BoardModel",
                column: "GameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardModel_GameModel_GameId",
                table: "BoardModel",
                column: "GameId",
                principalTable: "GameModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
