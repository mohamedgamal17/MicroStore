using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    public partial class RefactorUserNameToUserIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Shipments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_UserName",
                table: "Shipments",
                newName: "IX_Shipments_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Shipments",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Shipments_UserId",
                table: "Shipments",
                newName: "IX_Shipments_UserName");
        }
    }
}
