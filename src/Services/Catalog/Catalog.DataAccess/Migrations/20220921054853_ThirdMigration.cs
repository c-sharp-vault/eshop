using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.DataAccess.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_Name",
                table: "CatalogItems",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CatalogItems_Name",
                table: "CatalogItems");
        }
    }
}
