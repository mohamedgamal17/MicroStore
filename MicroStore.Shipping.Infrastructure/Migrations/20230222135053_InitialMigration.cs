using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SettingsEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    Address_CustomerName = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false, defaultValue: ""),
                    Address_CustomerPhone = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false, defaultValue: ""),
                    Address_CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Address_Zip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Address_AddressLine1 = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false, defaultValue: ""),
                    Address_AddressLine2 = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false, defaultValue: ""),
                    ShipmentExternalId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true, defaultValue: ""),
                    ShipmentLabelExternalId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true, defaultValue: ""),
                    TrackingNumber = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: true, defaultValue: ""),
                    SystemName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true, defaultValue: ""),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingSystems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(265)", maxLength: 265, nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight_Value = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Weight_Unit = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Dimension_Width = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Dimension_Lenght = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Height_Width = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    Dimension_Unit = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ShipmentId = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentItem_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingsEntity_ProviderKey",
                table: "SettingsEntity",
                column: "ProviderKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItem_Name",
                table: "ShipmentItem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItem_ProductId",
                table: "ShipmentItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItem_ShipmentId",
                table: "ShipmentItem",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentItem_Sku",
                table: "ShipmentItem",
                column: "Sku");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderId",
                table: "Shipments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderNumber",
                table: "Shipments",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentExternalId",
                table: "Shipments",
                column: "ShipmentExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ShipmentLabelExternalId",
                table: "Shipments",
                column: "ShipmentLabelExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_SystemName",
                table: "Shipments",
                column: "SystemName");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_TrackingNumber",
                table: "Shipments",
                column: "TrackingNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_UserId",
                table: "Shipments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingSystems_Name",
                table: "ShippingSystems",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingsEntity");

            migrationBuilder.DropTable(
                name: "ShipmentItem");

            migrationBuilder.DropTable(
                name: "ShippingSystems");

            migrationBuilder.DropTable(
                name: "Shipments");
        }
    }
}
