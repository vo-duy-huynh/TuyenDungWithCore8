using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddClUserIdReceive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "JobPostTemps",
                newName: "UserIdSend");

            migrationBuilder.AddColumn<string>(
                name: "UserIdReceive",
                table: "JobPostTemps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdReceive",
                table: "JobPostTemps");

            migrationBuilder.RenameColumn(
                name: "UserIdSend",
                table: "JobPostTemps",
                newName: "UserId");
        }
    }
}
