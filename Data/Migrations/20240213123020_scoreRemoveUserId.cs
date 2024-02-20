using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Data.Migrations
{
    /// <inheritdoc />
    public partial class scoreRemoveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreModel_AspNetUsers_UserId",
                table: "ScoreModel");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ScoreModel",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreModel_UserId",
                table: "ScoreModel",
                newName: "IX_ScoreModel_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreModel_AspNetUsers_ApplicationUserId",
                table: "ScoreModel",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreModel_AspNetUsers_ApplicationUserId",
                table: "ScoreModel");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "ScoreModel",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreModel_ApplicationUserId",
                table: "ScoreModel",
                newName: "IX_ScoreModel_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreModel_AspNetUsers_UserId",
                table: "ScoreModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
