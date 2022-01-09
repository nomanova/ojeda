using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class RemoveAttachmentThumbnail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                schema: "app",
                table: "AssetAttachments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail",
                schema: "app",
                table: "AssetAttachments",
                type: "bytea",
                nullable: true);
        }
    }
}
