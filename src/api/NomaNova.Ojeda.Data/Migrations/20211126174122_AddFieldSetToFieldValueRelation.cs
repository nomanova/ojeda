using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddFieldSetToFieldValueRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_FieldSets_FieldSetId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_FieldSets_FieldSetId",
                table: "FieldValues",
                column: "FieldSetId",
                principalTable: "FieldSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_FieldSets_FieldSetId",
                table: "FieldValues");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_FieldSets_FieldSetId",
                table: "FieldValues",
                column: "FieldSetId",
                principalTable: "FieldSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
