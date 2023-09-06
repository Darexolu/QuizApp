using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class newproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ans1a",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Ans1b",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Ans1a",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Answers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Ans1b",
                table: "Answers",
                newName: "Text");

            migrationBuilder.AddColumn<double>(
                name: "PercentageCorrect",
                table: "Questions",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Answers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "PercentageCorrect",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Answers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Answers",
                newName: "Ans1b");

            migrationBuilder.AddColumn<string>(
                name: "Ans1a",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ans1b",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ans1a",
                table: "Answers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
