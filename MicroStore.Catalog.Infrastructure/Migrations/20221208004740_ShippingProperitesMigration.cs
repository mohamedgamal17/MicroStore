using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class ShippingProperitesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Height_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");

            migrationBuilder.AddColumn<double>(
                name: "Height_Value",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Lenght_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");

            migrationBuilder.AddColumn<double>(
                name: "Lenght_Value",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Weight_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");

            migrationBuilder.AddColumn<double>(
                name: "Weight_Value",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Width_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");

            migrationBuilder.AddColumn<double>(
                name: "Width_Value",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height_Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Height_Value",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Lenght_Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Lenght_Value",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight_Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight_Value",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Width_Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Width_Value",
                table: "Products");
        }
    }
}
