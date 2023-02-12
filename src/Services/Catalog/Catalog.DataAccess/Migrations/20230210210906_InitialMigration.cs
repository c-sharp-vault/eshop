using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.CreateTable(
                name: "CatalogBrands",
                schema: "Catalog",
                columns: table => new
                {
                    CatalogBrandID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrands", x => x.CatalogBrandID);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTypes",
                schema: "Catalog",
                columns: table => new
                {
                    CatalogTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTypes", x => x.CatalogTypeID);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                schema: "Catalog",
                columns: table => new
                {
                    CatalogItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "placeholder.png"),
                    AvailableStock = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RestockThreshold = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    MaxStockThreshold = table.Column<int>(type: "int", nullable: false, defaultValue: 1000),
                    CatalogTypeID = table.Column<int>(type: "int", nullable: false),
                    CatalogBrandID = table.Column<int>(type: "int", nullable: false),
                    OnReorder = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.CatalogItemID);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogBrands_CatalogBrandID",
                        column: x => x.CatalogBrandID,
                        principalSchema: "Catalog",
                        principalTable: "CatalogBrands",
                        principalColumn: "CatalogBrandID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogItems_CatalogTypes_CatalogTypeID",
                        column: x => x.CatalogTypeID,
                        principalSchema: "Catalog",
                        principalTable: "CatalogTypes",
                        principalColumn: "CatalogTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "CatalogBrands",
                columns: new[] { "CatalogBrandID", "Brand", "CreatedBy", "CreatedOn", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "N/A", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(6636), "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(7280) },
                    { 2, "Coca-Cola", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(7917), "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(8524) },
                    { 3, "Terrabusi", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(9144), "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 442, DateTimeKind.Local).AddTicks(9718) },
                    { 4, "Marlboro", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(260), "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(855) },
                    { 5, "Quilmes", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(1463), "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(2064) }
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "CatalogTypes",
                columns: new[] { "CatalogTypeID", "CreatedBy", "CreatedOn", "Type", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(4604), "N/A", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(5216) },
                    { 2, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(5824), "Gaseosas", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(6427) },
                    { 3, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(7023), "Cigarrillos", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(7618) },
                    { 4, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(8212), "Alfajores & Obleas", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(8806) },
                    { 5, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(9401), "Cervezas", "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(9993) }
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "CatalogItems",
                columns: new[] { "CatalogItemID", "CatalogBrandID", "CatalogTypeID", "CreatedBy", "CreatedOn", "Description", "MaxStockThreshold", "Name", "PictureFileName", "Price", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 2, 2, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(8389), "", 1, "Sin Azúcar 1.5l", "placeholder.png", 0m, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(8999) },
                    { 2, 5, 5, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 443, DateTimeKind.Local).AddTicks(9612), "", 1, "Clásica 500ml", "placeholder.png", 0m, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(217) },
                    { 3, 3, 4, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(816), "", 1, "Tita", "placeholder.png", 0m, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(1410) },
                    { 4, 4, 3, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(1999), "", 1, "Ice Blast 8", "placeholder.png", 0m, "Fedex", new DateTime(2023, 2, 10, 18, 9, 6, 444, DateTimeKind.Local).AddTicks(2597) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogBrandID",
                schema: "Catalog",
                table: "CatalogItems",
                column: "CatalogBrandID");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CatalogTypeID",
                schema: "Catalog",
                table: "CatalogItems",
                column: "CatalogTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_Name",
                schema: "Catalog",
                table: "CatalogItems",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogItems",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "CatalogBrands",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "CatalogTypes",
                schema: "Catalog");
        }
    }
}
