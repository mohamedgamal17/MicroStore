using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class RefactorOrderStateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStateEntity_RejectedBy",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "AcceptenceDate",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "RejectedBy",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "OrderStateEntity");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "OrderStateEntity",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "RejectionDate",
                table: "OrderStateEntity",
                newName: "PaymentAcceptedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "OrderStateEntity",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "PaymentAcceptedDate",
                table: "OrderStateEntity",
                newName: "RejectionDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptenceDate",
                table: "OrderStateEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedBy",
                table: "OrderStateEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "OrderStateEntity",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_RejectedBy",
                table: "OrderStateEntity",
                column: "RejectedBy");
        }
    }
}
