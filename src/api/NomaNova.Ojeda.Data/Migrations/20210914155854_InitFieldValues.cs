using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class InitFieldValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldValues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AssetId = table.Column<string>(type: "text", nullable: true),
                    FieldSetId = table.Column<string>(type: "text", nullable: true),
                    FieldId = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldValues_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldValues_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FieldValues_FieldSets_FieldSetId",
                        column: x => x.FieldSetId,
                        principalTable: "FieldSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_AssetId",
                table: "FieldValues",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_FieldId",
                table: "FieldValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_FieldSetId",
                table: "FieldValues",
                column: "FieldSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldValues");
        }
    }
}
