using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Geographic.Application.Migrations
{
    public partial class FullyAuditedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "StateProvinces",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "StateProvinces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "StateProvinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "StateProvinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "StateProvinces",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "StateProvinces",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StateProvinces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "StateProvinces",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "StateProvinces",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Countries",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "Countries",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "StateProvinces");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Countries");
        }
    }
}
