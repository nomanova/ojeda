using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NomaNova.Ojeda.Data.Migrations
{
    public partial class AddAssetIdTypeToAssetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetIdTypeId",
                table: "AssetTypes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_AssetIdTypeId",
                table: "AssetTypes",
                column: "AssetIdTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetTypes_AssetIdTypes_AssetIdTypeId",
                table: "AssetTypes",
                column: "AssetIdTypeId",
                principalTable: "AssetIdTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetTypes_AssetIdTypes_AssetIdTypeId",
                table: "AssetTypes");

            migrationBuilder.DropIndex(
                name: "IX_AssetTypes_AssetIdTypeId",
                table: "AssetTypes");

            migrationBuilder.DropColumn(
                name: "AssetIdTypeId",
                table: "AssetTypes");
        }
    }
}
