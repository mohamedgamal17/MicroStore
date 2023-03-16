using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    public partial class RefactorUserNameToUserIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "PaymentRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_UserName",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PaymentRequests",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentRequests_UserId",
                table: "PaymentRequests",
                newName: "IX_PaymentRequests_UserName");
        }
    }
}
