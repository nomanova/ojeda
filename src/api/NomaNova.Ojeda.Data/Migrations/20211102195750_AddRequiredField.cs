using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddRequiredField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "FieldSetFields",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "FieldSetFields");
        }
    }
}
