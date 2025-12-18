using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class RefactorCustomerIdToUserNameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OrderStateEntity",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStateEntity_UserId",
                table: "OrderStateEntity",
                newName: "IX_OrderStateEntity_UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "OrderStateEntity",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStateEntity_UserName",
                table: "OrderStateEntity",
                newName: "IX_OrderStateEntity_UserId");
        }
    }
}
