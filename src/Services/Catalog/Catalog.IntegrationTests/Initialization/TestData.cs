using System;
using Catalog.Core.Models;
using System.Collections.Generic;

namespace Catalog.IntegrationTests.Initialization {
	internal static class TestData {
		public static string CurrentUser = "TestRunner";
		public static DateTime CurrentDateTime = DateTime.Today;

		public static List<CatalogBrand> CatalogBrands =>
			new List<CatalogBrand>() {
				new CatalogBrand() { CatalogBrandID = 1, Brand = "N/A" },
				new CatalogBrand() { CatalogBrandID = 2, Brand = "Coca-Cola" },
				new CatalogBrand() { CatalogBrandID = 3, Brand = "Terrabusi" },
				new CatalogBrand() { CatalogBrandID = 4, Brand = "Marlboro" },
				new CatalogBrand() { CatalogBrandID = 5, Brand = "Quilmes" }
			};


		public static List<CatalogType> CatalogTypes =>
			new List<CatalogType>() {
				new CatalogType() { CatalogTypeID = 1, Type = "N/A" },
				new CatalogType() { CatalogTypeID = 2, Type = "Gaseosas" },
				new CatalogType() { CatalogTypeID = 3, Type = "Cigarrillos" },
				new CatalogType() { CatalogTypeID = 4, Type = "Alfajores & Obleas" },
				new CatalogType() { CatalogTypeID = 5, Type = "Cervezas" }
			};

		public static List<CatalogItem> CatalogItems =>
			new List<CatalogItem>() {
				new CatalogItem() { CatalogItemID = 1, Name = "Sin Azúcar 1.5l", CatalogBrandID = 2, CatalogTypeID = 2, RestockThreshold = 10 },
				new CatalogItem() { CatalogItemID = 2, Name = "Clásica 500ml", CatalogBrandID = 5, CatalogTypeID = 5, RestockThreshold = 10 },
				new CatalogItem() { CatalogItemID = 3, Name = "Tita", CatalogBrandID = 3, CatalogTypeID = 4, RestockThreshold = 10 },
				new CatalogItem() { CatalogItemID = 4, Name = "Ice Blast 8", CatalogBrandID = 4, CatalogTypeID = 3, RestockThreshold = 10 }
			};
	}
}