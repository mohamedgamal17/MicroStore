using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TransctionId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true),
                    PaymentGateway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CapturedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FaultAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FaultReason = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_OrderId",
                table: "PaymentRequests",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_OrderNumber",
                table: "PaymentRequests",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_TransctionId",
                table: "PaymentRequests",
                column: "TransctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequests");
        }
    }
}
