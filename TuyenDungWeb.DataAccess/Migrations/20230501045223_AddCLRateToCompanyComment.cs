using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuyenDungWeb.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCLRateToCompanyComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "CompanyComments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "CompanyComments");
        }
    }
}
