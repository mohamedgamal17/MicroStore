using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Inventory.Infrastructure.Migrations
{
    public partial class RefactorCustomerIdToUserNameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserName",
                table: "Orders",
                newName: "IX_Orders_UserId");
        }
    }
}
