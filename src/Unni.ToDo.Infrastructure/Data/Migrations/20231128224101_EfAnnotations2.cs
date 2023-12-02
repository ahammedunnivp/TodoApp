using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unni.ToDo.API.Migrations
{
    /// <inheritdoc />
    public partial class EfAnnotations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "ToDoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "ToDoItems");
        }
    }
}
