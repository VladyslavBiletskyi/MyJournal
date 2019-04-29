using Microsoft.EntityFrameworkCore.Metadata;
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

            migrationBuilder.DropTable(
                name: "Attends");

            migrationBuilder.RenameColumn(
                name: "AttendId",
                table: "Marks",
                newName: "LessonSkipId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_AttendId",
                table: "Marks",
                newName: "IX_Marks_LessonSkipId");

            migrationBuilder.CreateTable(
                name: "LessonSkips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(nullable: true),
                    LessonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonSkips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonSkips_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonSkips_ApplicationUser_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonSkips_LessonId",
                table: "LessonSkips",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonSkips_StudentId",
                table: "LessonSkips",
                column: "StudentId");

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

            migrationBuilder.DropTable(
                name: "LessonSkips");

            migrationBuilder.RenameColumn(
                name: "LessonSkipId",
                table: "Marks",
                newName: "AttendId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_LessonSkipId",
                table: "Marks",
                newName: "IX_Marks_AttendId");

            migrationBuilder.CreateTable(
                name: "Attends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LessonId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attends_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attends_ApplicationUser_StudentId",
                        column: x => x.StudentId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attends_LessonId",
                table: "Attends",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Attends_StudentId",
                table: "Attends",
                column: "StudentId");

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
