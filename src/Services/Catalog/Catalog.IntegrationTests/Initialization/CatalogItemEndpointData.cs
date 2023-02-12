
using Catalog.Core.Models;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	internal class CatalogItemEndpointData : TestDataBase {

		protected override string[] Tables =>
			new string[] {
				 $"Catalog.{nameof(CatalogItem)}"
			};

		protected override async Task SeedData() {
			await ProcessInsert<CatalogItem>(TestData.CatalogItems);
		}
	}
}
