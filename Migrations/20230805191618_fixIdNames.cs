using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCalendar.Migrations
{
    /// <inheritdoc />
    public partial class fixIdNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarAnnotations",
                table: "CalendarAnnotations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "IdUser");

            migrationBuilder.AddColumn<int>(
                name: "IdCalendar",
                table: "CalendarAnnotations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarAnnotations",
                table: "CalendarAnnotations",
                column: "IdCalendar");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarAnnotations_Id",
                table: "CalendarAnnotations",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarAnnotations",
                table: "CalendarAnnotations");

            migrationBuilder.DropIndex(
                name: "IX_CalendarAnnotations_Id",
                table: "CalendarAnnotations");

            migrationBuilder.DropColumn(
                name: "IdCalendar",
                table: "CalendarAnnotations");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Users",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarAnnotations",
                table: "CalendarAnnotations",
                column: "Id");
        }
    }
}
