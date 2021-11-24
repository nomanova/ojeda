using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddAssetName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Assets",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Assets");
        }
    }
}
