using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class OrderRefactorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStateEntity_CancelledBy",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "FaultDate",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "FaultReason",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "PaymentAcceptedDate",
                table: "OrderStateEntity");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxCost",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "TaxCost",
                table: "OrderStateEntity");

            migrationBuilder.AddColumn<string>(
                name: "CancelledBy",
                table: "OrderStateEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FaultDate",
                table: "OrderStateEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaultReason",
                table: "OrderStateEntity",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentAcceptedDate",
                table: "OrderStateEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_CancelledBy",
                table: "OrderStateEntity",
                column: "CancelledBy");
        }
    }
}
