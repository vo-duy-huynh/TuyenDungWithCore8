using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddClUserIdForJobPostTemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobPostTemps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobPostTemps");
        }
    }
}
