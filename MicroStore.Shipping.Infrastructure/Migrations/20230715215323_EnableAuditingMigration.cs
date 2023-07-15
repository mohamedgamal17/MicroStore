using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Shipping.Infrastructure.Migrations
{
    public partial class EnableAuditingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ShippingSystems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ShippingSystems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ShippingSystems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "ShippingSystems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Shipments",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Shipments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Shipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "Shipments",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ShippingSystems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ShippingSystems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ShippingSystems");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ShippingSystems");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Shipments");
        }
    }
}
