using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedTagAndCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CompanyEmail", "Content", "Location", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "fpt@gmail.com", "No", "326 Hai Bà Trưng, Ba Đình, Hà Nội", "FPT", "0987654321" },
                    { 2, "intel@gmail.com", "No", "327 Hiệp Phú, Thủ Đức, Hồ Chí Minh", "Intel", "0987654321" },
                    { 3, "viecombank@gmail.com", "No", "123 Hải Thượng Lãn Ông, Đà Nẵng", "Vietcombank", "0987654321" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "DisplayName", "Name" },
                values: new object[,]
                {
                    { 1, null, "C#" },
                    { 2, null, "Java" },
                    { 3, null, "Python" },
                    { 4, null, "C++" },
                    { 5, null, "PHP" },
                    { 6, null, "Ruby" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
