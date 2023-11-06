using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class mergedGameAndBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameModel_BoardModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TileModel_BoardModel_BoardId",
                table: "TileModel");

            migrationBuilder.DropTable(
                name: "BoardModel");

            migrationBuilder.DropIndex(
                name: "IX_GameModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "GameModel");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "TileModel",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_TileModel_BoardId",
                table: "TileModel",
                newName: "IX_TileModel_GameId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameEnded",
                table: "GameModel",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "BoardSize",
                table: "GameModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BombPercentage",
                table: "GameModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "GameModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TileModel_GameModel_GameId",
                table: "TileModel",
                column: "GameId",
                principalTable: "GameModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TileModel_GameModel_GameId",
                table: "TileModel");

            migrationBuilder.DropColumn(
                name: "BoardSize",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "BombPercentage",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "GameModel");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "TileModel",
                newName: "BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_TileModel_GameId",
                table: "TileModel",
                newName: "IX_TileModel_BoardId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameEnded",
                table: "GameModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "GameModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardSize = table.Column<int>(type: "int", nullable: false),
                    BombPercentage = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardModel", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_TileModel_BoardModel_BoardId",
                table: "TileModel",
                column: "BoardId",
                principalTable: "BoardModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
