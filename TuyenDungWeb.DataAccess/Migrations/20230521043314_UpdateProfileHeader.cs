using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MatriculationDate",
                table: "ProfileHeaders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProfileHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProfileHeaders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MatriculationDate",
                table: "ProfileHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
