using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class RenameProductImageColumnMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ProductImage",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ProductImage",
                newName: "ImagePath");
        }
    }
}
