using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class list : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ans",
                table: "Answers",
                newName: "Ans1b");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ans1a",
                table: "Answers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Ans1a",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Ans1b",
                table: "Answers",
                newName: "Ans");
        }
    }
}
