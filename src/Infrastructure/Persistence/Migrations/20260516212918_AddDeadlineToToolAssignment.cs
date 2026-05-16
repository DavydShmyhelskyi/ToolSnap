using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeadlineToToolAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fcm_token",
                table: "users",
                type: "varchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "due_at",
                table: "tool_assignments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "reminder_sent_at",
                table: "tool_assignments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fcm_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "due_at",
                table: "tool_assignments");

            migrationBuilder.DropColumn(
                name: "reminder_sent_at",
                table: "tool_assignments");
        }
    }
}
