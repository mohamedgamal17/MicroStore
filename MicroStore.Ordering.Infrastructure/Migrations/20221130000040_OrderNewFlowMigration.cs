using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class OrderNewFlowMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStateEntity_ShippmentId",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ConfirmationDate",
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

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OrderStateEntity",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "ShippmentId",
                table: "OrderStateEntity",
                newName: "ShipmentSystem");

            migrationBuilder.RenameColumn(
                name: "CancelledBy",
                table: "OrderStateEntity",
                newName: "ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStateEntity_CancelledBy",
                table: "OrderStateEntity",
                newName: "IX_OrderStateEntity_ShipmentId");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentState",
                table: "OrderStateEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "OrderItemEntity",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "TaxCost",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "OrderItemEntity");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderStateEntity",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "ShipmentSystem",
                table: "OrderStateEntity",
                newName: "ShippmentId");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "OrderStateEntity",
                newName: "CancelledBy");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStateEntity_ShipmentId",
                table: "OrderStateEntity",
                newName: "IX_OrderStateEntity_CancelledBy");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentState",
                table: "OrderStateEntity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationDate",
                table: "OrderStateEntity",
                type: "datetime2",
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
                name: "IX_OrderStateEntity_ShippmentId",
                table: "OrderStateEntity",
                column: "ShippmentId");
        }
    }
}
