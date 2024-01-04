using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class RefactorProductManufacturerRelationshipMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductManufacturer");

            migrationBuilder.CreateTable(
                name: "ManufacturerProduct",
                columns: table => new
                {
                    ManufacturersId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    ProductsId = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerProduct", x => new { x.ManufacturersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ManufacturerProduct_Manufacturers_ManufacturersId",
                        column: x => x.ManufacturersId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManufacturerProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerProduct_ProductsId",
                table: "ManufacturerProduct",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturerProduct");

            migrationBuilder.CreateTable(
                name: "ProductManufacturer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ManufacturerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductManufacturer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductManufacturer_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductManufacturer_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductManufacturer_ManufacturerId",
                table: "ProductManufacturer",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductManufacturer_ProductId",
                table: "ProductManufacturer",
                column: "ProductId");
        }
    }
}
