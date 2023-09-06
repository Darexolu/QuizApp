using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class rolesadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataAnalysis",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Development",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Support",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataAnalysis",
                table: "Departments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Development",
                table: "Departments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Support",
                table: "Departments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId1",
                table: "AspNetUsers",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
