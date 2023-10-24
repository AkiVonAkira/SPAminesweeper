using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAmineseweeper.Migrations
{
    /// <inheritdoc />
    public partial class burah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "BoardModel");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "BoardModel",
                newName: "BoardSize");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoardSize",
                table: "BoardModel",
                newName: "Width");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "BoardModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
