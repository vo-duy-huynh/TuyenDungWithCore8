using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedJobType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fulltime" },
                    { 2, "Parttime" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JobTypes",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
