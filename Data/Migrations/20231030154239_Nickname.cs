using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class Nickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerModel");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "BoardModel");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "BoardModel");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "BoardModel",
                newName: "BoardSize");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameStarted",
                table: "GameModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GameModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ScoreModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HighScore = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TileModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    IsMine = table.Column<bool>(type: "bit", nullable: false),
                    AdjacentMines = table.Column<int>(type: "int", nullable: false),
                    IsRevealed = table.Column<bool>(type: "bit", nullable: false),
                    IsFlagged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TileModel_BoardModel_BoardId",
                        column: x => x.BoardId,
                        principalTable: "BoardModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_BoardId",
                table: "GameModel",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_UserId",
                table: "GameModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreModel_UserId",
                table: "ScoreModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TileModel_BoardId",
                table: "TileModel",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameModel_AspNetUsers_UserId",
                table: "GameModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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
                name: "FK_GameModel_AspNetUsers_UserId",
                table: "GameModel");

            migrationBuilder.DropForeignKey(
                name: "FK_GameModel_BoardModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropTable(
                name: "ScoreModel");

            migrationBuilder.DropTable(
                name: "TileModel");

            migrationBuilder.DropIndex(
                name: "IX_GameModel_BoardId",
                table: "GameModel");

            migrationBuilder.DropIndex(
                name: "IX_GameModel_UserId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameModel");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "BoardSize",
                table: "BoardModel",
                newName: "Width");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameStarted",
                table: "GameModel",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameEnded",
                table: "GameModel",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "BoardModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "BoardModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlayerModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HighScore = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerModel", x => x.Id);
                });
        }
    }
}
