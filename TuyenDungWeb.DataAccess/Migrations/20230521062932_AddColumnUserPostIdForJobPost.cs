using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnUserPostIdForJobPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ProfileHeaders");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "ProfileHeaders",
                newName: "UserReceiveId");

            migrationBuilder.AddColumn<string>(
                name: "UserPostId",
                table: "JobPosts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPostId",
                table: "JobPosts");

            migrationBuilder.RenameColumn(
                name: "UserReceiveId",
                table: "ProfileHeaders",
                newName: "SessionId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ProfileHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
