using Microsoft.EntityFrameworkCore.Migrations;

namespace MyJournal.Domain.Migrations
{
    public partial class MarksExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Marks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Marks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_LessonId",
                table: "Marks",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_StudentId",
                table: "Marks",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Lessons_LessonId",
                table: "Marks",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_ApplicationUser_StudentId",
                table: "Marks",
                column: "StudentId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Lessons_LessonId",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_ApplicationUser_StudentId",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_LessonId",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_StudentId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Marks");
        }
    }
}
