using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    public partial class RefactorCustomerIdToUserNameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "PaymentRequests",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_CustomerId",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "PaymentRequests",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_UserName",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_CustomerId");
        }
    }
}
