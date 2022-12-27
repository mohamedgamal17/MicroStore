using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroStore.Catalog.Infrastructure.Migrations
{
    public partial class FixProductDimensionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height_Unit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Lenght_Unit",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Width_Unit",
                table: "Products",
                newName: "Dimension_Unit");

            migrationBuilder.RenameColumn(
                name: "Width_Value",
                table: "Products",
                newName: "Height_Width");

            migrationBuilder.RenameColumn(
                name: "Lenght_Value",
                table: "Products",
                newName: "Dimension_Width");

            migrationBuilder.RenameColumn(
                name: "Height_Value",
                table: "Products",
                newName: "Dimension_Lenght");

            migrationBuilder.AlterColumn<int>(
                name: "Weight_Unit",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "none");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "OldPrice",
                table: "Products",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Dimension_Unit",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "none");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dimension_Unit",
                table: "Products",
                newName: "Width_Unit");

            migrationBuilder.RenameColumn(
                name: "Height_Width",
                table: "Products",
                newName: "Width_Value");

            migrationBuilder.RenameColumn(
                name: "Dimension_Width",
                table: "Products",
                newName: "Lenght_Value");

            migrationBuilder.RenameColumn(
                name: "Dimension_Lenght",
                table: "Products",
                newName: "Height_Value");

            migrationBuilder.AlterColumn<string>(
                name: "Weight_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "OldPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Width_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Height_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");

            migrationBuilder.AddColumn<string>(
                name: "Lenght_Unit",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "none");
        }
    }
}
