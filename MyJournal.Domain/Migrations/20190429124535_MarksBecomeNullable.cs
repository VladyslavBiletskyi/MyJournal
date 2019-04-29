using Microsoft.EntityFrameworkCore.Migrations;

namespace MyJournal.Domain.Migrations
{
    public partial class MarksBecomeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "Marks",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "Marks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
