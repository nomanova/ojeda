using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "AssetIdTypes",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WithManualEntry = table.Column<bool>(type: "boolean", nullable: false),
                    Properties = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetIdTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldSets",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AssetIdTypeId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetTypes_AssetIdTypes_AssetIdTypeId",
                        column: x => x.AssetIdTypeId,
                        principalSchema: "app",
                        principalTable: "AssetIdTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FieldSetFields",
                schema: "app",
                columns: table => new
                {
                    FieldId = table.Column<string>(type: "text", nullable: false),
                    FieldSetId = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSetFields", x => new { x.FieldId, x.FieldSetId });
                    table.ForeignKey(
                        name: "FK_FieldSetFields_Fields_FieldId",
                        column: x => x.FieldId,
                        principalSchema: "app",
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldSetFields_FieldSets_FieldSetId",
                        column: x => x.FieldSetId,
                        principalSchema: "app",
                        principalTable: "FieldSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AssetId = table.Column<string>(type: "text", nullable: false),
                    AssetTypeId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalSchema: "app",
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypeFieldSets",
                schema: "app",
                columns: table => new
                {
                    FieldSetId = table.Column<string>(type: "text", nullable: false),
                    AssetTypeId = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypeFieldSets", x => new { x.FieldSetId, x.AssetTypeId });
                    table.ForeignKey(
                        name: "FK_AssetTypeFieldSets_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalSchema: "app",
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetTypeFieldSets_FieldSets_FieldSetId",
                        column: x => x.FieldSetId,
                        principalSchema: "app",
                        principalTable: "FieldSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetAttachments",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssetId = table.Column<string>(type: "text", nullable: true),
                    DisplayFileName = table.Column<string>(type: "text", nullable: true),
                    StorageFileName = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Thumbnail = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetAttachments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "app",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldValues",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AssetId = table.Column<string>(type: "text", nullable: true),
                    FieldSetId = table.Column<string>(type: "text", nullable: true),
                    FieldId = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldValues_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "app",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldValues_Fields_FieldId",
                        column: x => x.FieldId,
                        principalSchema: "app",
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FieldValues_FieldSets_FieldSetId",
                        column: x => x.FieldSetId,
                        principalSchema: "app",
                        principalTable: "FieldSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetAttachments_AssetId",
                schema: "app",
                table: "AssetAttachments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetId",
                schema: "app",
                table: "Assets",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetTypeId",
                schema: "app",
                table: "Assets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypeFieldSets_AssetTypeId",
                schema: "app",
                table: "AssetTypeFieldSets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_AssetIdTypeId",
                schema: "app",
                table: "AssetTypes",
                column: "AssetIdTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSetFields_FieldSetId",
                schema: "app",
                table: "FieldSetFields",
                column: "FieldSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_AssetId",
                schema: "app",
                table: "FieldValues",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_FieldId",
                schema: "app",
                table: "FieldValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_FieldSetId",
                schema: "app",
                table: "FieldValues",
                column: "FieldSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetAttachments",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AssetTypeFieldSets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FieldSetFields",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FieldValues",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Assets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Fields",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FieldSets",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AssetTypes",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AssetIdTypes",
                schema: "app");
        }
    }
}
