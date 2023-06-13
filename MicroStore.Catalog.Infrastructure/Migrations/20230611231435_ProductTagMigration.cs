using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class ProductTagMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(650)", maxLength: 650, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductTag",
                columns: table => new
                {
                    ProductTagsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductsId = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductTag", x => new { x.ProductTagsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductTag_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                        column: x => x.ProductTagsId,
                        principalTable: "ProductTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductTag_ProductsId",
                table: "ProductProductTag",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_Name",
                table: "ProductTags",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductTag");

            migrationBuilder.DropTable(
                name: "ProductTags");
        }
    }
}
