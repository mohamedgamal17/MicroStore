using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Payment.Application.Migrations
{
    public partial class EnableAuditingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "PaymentSystems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "PaymentSystems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PaymentSystems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "PaymentSystems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PaymentRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "PaymentRequests",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "PaymentSystems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "PaymentSystems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PaymentSystems");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "PaymentSystems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "PaymentRequests");
        }
    }
}
