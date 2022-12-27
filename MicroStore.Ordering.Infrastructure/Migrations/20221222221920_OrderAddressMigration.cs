using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Ordering.Infrastructure.Migrations
{
    public partial class OrderAddressMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStateEntity_BillingAddressId",
                table: "OrderStateEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrderStateEntity_ShippingAddressId",
                table: "OrderStateEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemEntity_ProductId",
                table: "OrderItemEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShipmentSystem",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "OrderItemEntity");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderItemEntity");

            migrationBuilder.RenameColumn(
                name: "ProductImage",
                table: "OrderItemEntity",
                newName: "Thumbnail");

            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "OrderStateEntity",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "TaxCost",
                table: "OrderStateEntity",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "SubTotal",
                table: "OrderStateEntity",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "ShippingCost",
                table: "OrderStateEntity",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_AddressLine1",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_AddressLine2",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_City",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_CountryCode",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Name",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Phone",
                table: "OrderStateEntity",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_PostalCode",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_State",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingAddress_Zip",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_AddressLine1",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_AddressLine2",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_CountryCode",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Name",
                table: "OrderStateEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Phone",
                table: "OrderStateEntity",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PostalCode",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_State",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Zip",
                table: "OrderStateEntity",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "OrderItemEntity",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ExternalProductId",
                table: "OrderItemEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderItemEntity",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "OrderItemEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemEntity_ExternalProductId",
                table: "OrderItemEntity",
                column: "ExternalProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemEntity_Sku",
                table: "OrderItemEntity",
                column: "Sku");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemEntity_ExternalProductId",
                table: "OrderItemEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemEntity_Sku",
                table: "OrderItemEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_AddressLine1",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_AddressLine2",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_City",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_CountryCode",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Name",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Phone",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_PostalCode",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_State",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "BillingAddress_Zip",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_AddressLine1",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_AddressLine2",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_CountryCode",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Name",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Phone",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PostalCode",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_State",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Zip",
                table: "OrderStateEntity");

            migrationBuilder.DropColumn(
                name: "ExternalProductId",
                table: "OrderItemEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderItemEntity");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "OrderItemEntity");

            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "OrderItemEntity",
                newName: "ProductImage");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxCost",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingCost",
                table: "OrderStateEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                table: "OrderStateEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ShipmentSystem",
                table: "OrderStateEntity",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAddressId",
                table: "OrderStateEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItemEntity",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "OrderItemEntity",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "OrderItemEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_BillingAddressId",
                table: "OrderStateEntity",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateEntity_ShippingAddressId",
                table: "OrderStateEntity",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemEntity_ProductId",
                table: "OrderItemEntity",
                column: "ProductId");
        }
    }
}
