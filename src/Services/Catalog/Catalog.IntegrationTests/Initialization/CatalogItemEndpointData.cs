
using Catalog.Core.Models;

namespace Catalog.IntegrationTests.Initialization {
	public class CatalogItemEndpointData : TestDataBase {

		protected override string[] Tables =>
			new string[] {
				$"Catalog.{nameof(CatalogBrand)}s",
				$"Catalog.{nameof(CatalogType)}s",
				$"Catalog.{nameof(CatalogItem)}s"
			};

		protected override void SeedData() {
			ProcessInsert<CatalogBrand>(TestData.CatalogBrands);
			ProcessInsert<CatalogType>(TestData.CatalogTypes);
			ProcessInsert<CatalogItem>(TestData.CatalogItems);
		}
	}
}
