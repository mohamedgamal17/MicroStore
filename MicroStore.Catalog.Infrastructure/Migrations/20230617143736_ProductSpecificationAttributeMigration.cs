using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class ProductSpecificationAttributeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecificationAttributes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(650)", maxLength: 650, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationAttributeOption",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SpecificationAttributeId = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationAttributeOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationAttributeOption_SpecificationAttributes_SpecificationAttributeId",
                        column: x => x.SpecificationAttributeId,
                        principalTable: "SpecificationAttributes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationAttribute",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AttributeId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OptionId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationAttribute_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSpecificationAttribute_SpecificationAttributeOption_OptionId",
                        column: x => x.OptionId,
                        principalTable: "SpecificationAttributeOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSpecificationAttribute_SpecificationAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "SpecificationAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationAttribute_AttributeId",
                table: "ProductSpecificationAttribute",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationAttribute_OptionId",
                table: "ProductSpecificationAttribute",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecificationAttribute_ProductId",
                table: "ProductSpecificationAttribute",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationAttributeOption_SpecificationAttributeId",
                table: "SpecificationAttributeOption",
                column: "SpecificationAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationAttributes_Name",
                table: "SpecificationAttributes",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSpecificationAttribute");

            migrationBuilder.DropTable(
                name: "SpecificationAttributeOption");

            migrationBuilder.DropTable(
                name: "SpecificationAttributes");
        }
    }
}
