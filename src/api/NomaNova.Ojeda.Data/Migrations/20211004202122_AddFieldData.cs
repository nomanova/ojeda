using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddFieldData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Fields",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Fields",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
