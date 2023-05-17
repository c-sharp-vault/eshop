
namespace Catalog.IntegrationTests.Services {
	internal static class TestURLs {
		static readonly string _baseURL = "/api/v1";

		public static class CatalogItem {
			public static string ReadSingleEndpoint = $"{_baseURL}/catalog-items/get-single";
			public static string ReadRangeEndpoint = $"{_baseURL}/catalog-items/get-range";
			public static string CreateSingleEndpoint = $"{_baseURL}/catalog-items/create-single";
			public static string CreateRangeEndpoint = $"{_baseURL}/catalog-items/create-range";
			public static string UpdateSingleEndpoint = $"{_baseURL}/catalog-items/update-single";
			public static string UpdateRangeEndpoint = $"{_baseURL}/catalog-items/update-range";
			public static string DeleteSingleEndpoint = $"{_baseURL}/catalog-items/remove-single";
			public static string DeleteRangeEndpoint = $"{_baseURL}/catalog-items/remove-range";
		}
	}
}