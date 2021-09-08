using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class InitFieldSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldSets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldSetFields",
                columns: table => new
                {
                    FieldId = table.Column<string>(type: "text", nullable: false),
                    FieldSetId = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSetFields", x => new { x.FieldId, x.FieldSetId });
                    table.ForeignKey(
                        name: "FK_FieldSetFields_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldSetFields_FieldSets_FieldSetId",
                        column: x => x.FieldSetId,
                        principalTable: "FieldSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldSetFields_FieldSetId",
                table: "FieldSetFields",
                column: "FieldSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldSetFields");

            migrationBuilder.DropTable(
                name: "FieldSets");
        }
    }
}
