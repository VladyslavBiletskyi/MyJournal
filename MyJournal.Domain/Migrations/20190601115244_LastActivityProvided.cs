using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyJournal.Domain.Migrations
{
    public partial class LastActivityProvided : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "ApplicationUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "ApplicationUser");
        }
    }
}
