using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    public partial class PaymentFlowRefactorMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaultReason",
                table: "PaymentRequests");

            migrationBuilder.RenameColumn(
                name: "OpenedAt",
                table: "PaymentRequests",
                newName: "RefundedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PaymentRequests",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PaymentRequests",
                newName: "TotalCost");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "PaymentRequests",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PaymentRequests",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "PaymentRequests",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "PaymentRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PaymentRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "PaymentRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "PaymentRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxCost",
                table: "PaymentRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "PaymentRequestProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequestProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRequestProduct_PaymentRequests_PaymentRequestId",
                        column: x => x.PaymentRequestId,
                        principalTable: "PaymentRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_CustomerId",
                table: "PaymentRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequestProduct_Name",
                table: "PaymentRequestProduct",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequestProduct_PaymentRequestId",
                table: "PaymentRequestProduct",
                column: "PaymentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequestProduct_ProductId",
                table: "PaymentRequestProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequestProduct_Sku",
                table: "PaymentRequestProduct",
                column: "Sku");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequestProduct");

            migrationBuilder.DropIndex(
                name: "IX_PaymentRequests_CustomerId",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "TaxCost",
                table: "PaymentRequests");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "PaymentRequests",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "RefundedAt",
                table: "PaymentRequests",
                newName: "OpenedAt");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "PaymentRequests",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "PaymentRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "PaymentRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265);

            migrationBuilder.AddColumn<string>(
                name: "FaultReason",
                table: "PaymentRequests",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: true);
        }
    }
}
