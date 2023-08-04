using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCalendar.Migrations
{
    /// <inheritdoc />
    public partial class addUserAgainBecauseThatOneBeforeWithoutAllThisTextFailedThisIsTheLastOneReally : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");
        }
    }
}
