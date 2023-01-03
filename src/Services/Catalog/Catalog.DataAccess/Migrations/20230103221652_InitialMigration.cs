using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    CatalogBrandID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    CatalogTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    CatalogItemID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PictureFileName = table.Column<string>(type: "text", nullable: true, defaultValue: "placeholder.png"),
                    AvailableStock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    RestockThreshold = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    MaxStockThreshold = table.Column<int>(type: "integer", nullable: false, defaultValue: 1000),
                    CatalogTypeID = table.Column<int>(type: "integer", nullable: false),
                    CatalogBrandID = table.Column<int>(type: "integer", nullable: false),
                    OnReorder = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    { 1, "N/A", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(1545), "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(2234) },
                    { 2, "Coca-Cola", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(2915), "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(3543) },
                    { 3, "Terrabusi", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(4182), "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(4810) },
                    { 4, "Marlboro", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(5441), "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(6057) },
                    { 5, "Quilmes", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(6688), "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 717, DateTimeKind.Local).AddTicks(7314) }
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "CatalogTypes",
                columns: new[] { "CatalogTypeID", "CreatedBy", "CreatedOn", "Type", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(9477), "N/A", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(117) },
                    { 2, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(752), "Gaseosas", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(1381) },
                    { 3, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(2016), "Cigarrillos", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(2643) },
                    { 4, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(3275), "Alfajores & Obleas", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(3903) },
                    { 5, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(4540), "Cervezas", "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 719, DateTimeKind.Local).AddTicks(5159) }
                });

            migrationBuilder.InsertData(
                schema: "Catalog",
                table: "CatalogItems",
                columns: new[] { "CatalogItemID", "CatalogBrandID", "CatalogTypeID", "CreatedBy", "CreatedOn", "Description", "MaxStockThreshold", "Name", "PictureFileName", "Price", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, 2, 2, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(3069), "", 1, "Sin Azúcar 1.5l", "placeholder.png", 0m, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(3715) },
                    { 2, 5, 5, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(4368), "", 1, "Clásica 500ml", "placeholder.png", 0m, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(4983) },
                    { 3, 3, 4, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(5623), "", 1, "Tita", "placeholder.png", 0m, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(6246) },
                    { 4, 4, 3, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(6870), "", 1, "Ice Blast 8", "placeholder.png", 0m, "Fedex", new DateTime(2023, 1, 3, 19, 16, 52, 718, DateTimeKind.Local).AddTicks(7488) }
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
