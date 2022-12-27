using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    public partial class ExternalShippingSystemMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingSystem",
                table: "ShippingSystem");

            migrationBuilder.RenameTable(
                name: "ShippingSystem",
                newName: "ShippingSystems");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingSystem_Name",
                table: "ShippingSystems",
                newName: "IX_ShippingSystems_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingSystems",
                table: "ShippingSystems",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingSystems",
                table: "ShippingSystems");

            migrationBuilder.RenameTable(
                name: "ShippingSystems",
                newName: "ShippingSystem");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingSystems_Name",
                table: "ShippingSystem",
                newName: "IX_ShippingSystem_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingSystem",
                table: "ShippingSystem",
                column: "Id");
        }
    }
}
