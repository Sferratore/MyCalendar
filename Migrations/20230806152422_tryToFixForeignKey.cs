using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCalendar.Migrations
{
    /// <inheritdoc />
    public partial class tryToFixForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarAnnotations_Users_Id",
                table: "CalendarAnnotations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CalendarAnnotations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarAnnotations_Id",
                table: "CalendarAnnotations",
                newName: "IX_CalendarAnnotations_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarAnnotations_Users_UserId",
                table: "CalendarAnnotations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarAnnotations_Users_UserId",
                table: "CalendarAnnotations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CalendarAnnotations",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarAnnotations_UserId",
                table: "CalendarAnnotations",
                newName: "IX_CalendarAnnotations_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarAnnotations_Users_Id",
                table: "CalendarAnnotations",
                column: "Id",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
