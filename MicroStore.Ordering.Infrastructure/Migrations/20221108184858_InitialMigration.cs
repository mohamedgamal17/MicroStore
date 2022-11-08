using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStateEntity",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillingAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ShippmentId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptenceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FaultReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FaultDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStateEntity", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderStateEntityCorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemEntity_OrderStateEntity_OrderStateEntityCorrelationId",
                        column: x => x.OrderStateEntityCorrelationId,
                        principalTable: "OrderStateEntity",
                        principalColumn: "CorrelationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemEntity_OrderStateEntityCorrelationId",
                table: "OrderItemEntity",
                column: "OrderStateEntityCorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemEntity_ProductId",
                table: "OrderItemEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_BillingAddressId",
                table: "OrderStateEntity",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_CancelledBy",
                table: "OrderStateEntity",
                column: "CancelledBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_OrderNumber",
                table: "OrderStateEntity",
                column: "OrderNumber",
                unique: true,
                filter: "[OrderNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_RejectedBy",
                table: "OrderStateEntity",
                column: "RejectedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_ShippingAddressId",
                table: "OrderStateEntity",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_ShippmentId",
                table: "OrderStateEntity",
                column: "ShippmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_UserId",
                table: "OrderStateEntity",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemEntity");

            migrationBuilder.DropTable(
                name: "OrderStateEntity");
        }
    }
}
