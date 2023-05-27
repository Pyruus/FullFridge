using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullFridge.API.Migrations
{
    /// <inheritdoc />
    public partial class typofix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DIslikes",
                table: "Recipes",
                newName: "Dislikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dislikes",
                table: "Recipes",
                newName: "DIslikes");
        }
    }
}
