using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddFieldToFieldValueRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_Fields_FieldId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_Fields_FieldId",
                table: "FieldValues",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_Fields_FieldId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_Fields_FieldId",
                table: "FieldValues",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
