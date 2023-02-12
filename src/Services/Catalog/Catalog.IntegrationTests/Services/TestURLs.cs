
namespace Catalog.IntegrationTests.Services {
	internal static class TestURLs {
		static readonly string _baseURL = "/api/v1";

		public static class CatalogItem {
			public static string CatalogItemsEndpoint = $"{_baseURL}/catalog/items";
		}
	}
}