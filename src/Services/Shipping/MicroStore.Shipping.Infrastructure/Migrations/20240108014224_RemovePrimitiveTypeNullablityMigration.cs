using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    public partial class RemovePrimitiveTypeNullablityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "Shipments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShipmentLabelExternalId",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShipmentExternalId",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Thumbnail",
                table: "ShipmentItem",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(600)",
                oldMaxLength: 600,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "Shipments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShipmentLabelExternalId",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ShipmentExternalId",
                table: "Shipments",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Thumbnail",
                table: "ShipmentItem",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(600)",
                oldMaxLength: 600,
                oldNullable: true,
                oldDefaultValue: "");
        }
    }
}
