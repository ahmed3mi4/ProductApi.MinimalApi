using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductApi.MinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProductConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Products_Name_NotBlank",
                table: "Products",
                sql: "LEN(LTRIM(RTRIM([Name]))) > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Products_Price_Positive",
                table: "Products",
                sql: "[Price] > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Products_Name_NotBlank",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Products_Price_Positive",
                table: "Products");
        }
    }
}
