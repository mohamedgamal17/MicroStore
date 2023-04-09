using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.IdentityProvider.Identity.Infrastructure.Migrations
{
    public partial class RefactorUserFirstNameAndLastNameMigraation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "IdentityModule",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "IdentityModule",
                table: "IdentityUsers");

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                schema: "IdentityModule",
                table: "IdentityUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                schema: "IdentityModule",
                table: "IdentityUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyName",
                schema: "IdentityModule",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "GivenName",
                schema: "IdentityModule",
                table: "IdentityUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "IdentityModule",
                table: "IdentityUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "IdentityModule",
                table: "IdentityUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
