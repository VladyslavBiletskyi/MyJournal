using Microsoft.EntityFrameworkCore.Migrations;

namespace MyJournal.Domain.Migrations
{
    public partial class GroupStudentRelationshipsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Teacher_GroupId",
                table: "ApplicationUser",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Teacher_GroupId",
                table: "ApplicationUser",
                column: "Teacher_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUser_Groups_Teacher_GroupId",
                table: "ApplicationUser",
                column: "Teacher_GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUser_Groups_Teacher_GroupId",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_Teacher_GroupId",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Teacher_GroupId",
                table: "ApplicationUser");
        }
    }
}
