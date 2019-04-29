using Microsoft.EntityFrameworkCore.Migrations;

namespace MyJournal.Domain.Migrations
{
    public partial class AttendsReworkedToLessonSkips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Attends_AttendId",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "AttendId",
                table: "Marks",
                newName: "LessonSkipId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_AttendId",
                table: "Marks",
                newName: "IX_Marks_LessonSkipId");

            migrationBuilder.RenameTable(
                name: "Attends",
                newName: "LessonSkips");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_LessonSkips_LessonSkipId",
                table: "Marks",
                column: "LessonSkipId",
                principalTable: "LessonSkips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_LessonSkips_LessonSkipId",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "LessonSkipId",
                table: "Marks",
                newName: "AttendId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_LessonSkipId",
                table: "Marks",
                newName: "IX_Marks_AttendId");

            migrationBuilder.RenameTable(
                name: "LessonSkips",
                newName: "Attends");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Attends_AttendId",
                table: "Marks",
                column: "AttendId",
                principalTable: "Attends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
