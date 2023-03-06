using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class RefactorProductEntityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFeaturedProduct",
                table: "ProductCategory");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Products",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeaturedProduct",
                table: "ProductCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
