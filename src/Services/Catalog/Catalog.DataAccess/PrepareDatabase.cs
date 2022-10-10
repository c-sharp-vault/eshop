using Catalog.Core.Models;
using Catalog.Infrastructure.Extensions.Linq;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Catalog.DataAccess {
	public class PrepareDatabase {
		private static ILogger<PrepareDatabase> _logger;
		private static IUnitOfWork _unitOfWork;
		private static IOptions<CatalogOptions> _options;
		private static IWebHostEnvironment _webHostEnvironment;

		public static async Task MigrateAndSeedAsync(IApplicationBuilder applicationBuilder) {
			using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope()) {

				_unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
				_logger = serviceScope.ServiceProvider.GetService<ILogger<PrepareDatabase>>();
				_options = serviceScope.ServiceProvider.GetService<IOptions<CatalogOptions>>();
				_webHostEnvironment = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

				bool useCustomizationData = _options.Value.UseCustomizationData;
				String contentRootPath = _webHostEnvironment.ContentRootPath;
				String picturePath = _webHostEnvironment.WebRootPath;

				AsyncRetryPolicy policy = CreatePolicy(_logger, nameof(PrepareDatabase));

				await policy.ExecuteAsync(async () => {

					_logger.LogInformation("--> Applying Migrations...");
					await _unitOfWork.MigrateAsync();

					if (!_unitOfWork.CatalogBrandRepository.AnyAsync().Result) {
						_logger.LogInformation("--> Seeding CatalogBrand records...");
						await _unitOfWork.CatalogBrandRepository.AddRangeAsync(
							useCustomizationData
							? GetCatalogBrandsFromFile(contentRootPath, _logger).Result
							: GetPreconfiguredCatalogBrands()
						);
						await _unitOfWork.CompleteAsync();
					}

					if (!_unitOfWork.CatalogTypeRepository.AnyAsync().Result) {
						_logger.LogInformation("--> Seeding CatalogType Records...");
						await _unitOfWork.CatalogTypeRepository.AddRangeAsync(
							useCustomizationData
							? GetCatalogTypesFromFile(contentRootPath, _logger).Result
							: GetPreconfiguredCatalogTypes()
						);
						await _unitOfWork.CompleteAsync();
					}

					if (!_unitOfWork.CatalogItemRepository.AnyAsync().Result) {
						_logger.LogInformation("--> Seeding CatalogItem records...");
						await _unitOfWork.CatalogItemRepository.AddRangeAsync(
							useCustomizationData
							? GetCatalogItemsFromFile(contentRootPath, _logger).Result
							: GetPreconfiguredCatalogItems().Result
						);
						await _unitOfWork.CompleteAsync();
					}
				});
			}
		}

		#region CatalogBrands

		private static async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsFromFile(String contentRootPath, ILogger<PrepareDatabase> logger) {
			//String csvFileCatalogBrandsPath = Path.Combine(contentRootPath, "SeedData", "Brands.csv");
			String csvFileCatalogBrandsPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogBrands.csv";

			if (!File.Exists(csvFileCatalogBrandsPath)) {
				return GetPreconfiguredCatalogBrands();
			}

			String[] csvHeaders;
			try {
				String[] requiredHeaders = { "catalogbrand" };
				csvHeaders = await GetHeaders(csvFileCatalogBrandsPath, requiredHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogBrands();
			}

			String[] fileLines = await File.ReadAllLinesAsync(csvFileCatalogBrandsPath);
			return fileLines.Skip(1)
							.SelectTry(x => CreateCatalogBrand(x))
							.OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null;})
							.Where(x => x != null);
		}

		private static CatalogBrand CreateCatalogBrand(String brand) {
			brand = brand.Trim('"').Trim();
			if (String.IsNullOrEmpty(brand)) {
				throw new Exception("CatalogBrand name empty");
			}
			return new CatalogBrand(brand);
		}

		private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands() {
			return new List<CatalogBrand>() {
				new CatalogBrand("Azure"),
				new CatalogBrand(".NET"),
				new CatalogBrand("Visual Studio"),
				new CatalogBrand("SQL Server"),
				new CatalogBrand("Other")
			};
		}

		#endregion

		#region CatalogTypes

		private static async Task<IEnumerable<CatalogType>> GetCatalogTypesFromFile(String contentRootPath, ILogger<PrepareDatabase> logger) {
			//String csvFileCatalogTypesPath = Path.Combine(contentRootPath, "SeedData", "Types.csv");
			String csvFileCatalogTypesPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogTypes.csv";

			if (!File.Exists(csvFileCatalogTypesPath)) {
				return GetPreconfiguredCatalogTypes();
			}

			String[] csvHeaders;
			try {
				String[] requiredHeaders = { "catalogtype" };
				csvHeaders = await GetHeaders(csvFileCatalogTypesPath, requiredHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogTypes();
			}

			String[] fileLines = await File.ReadAllLinesAsync(csvFileCatalogTypesPath);
			return fileLines.Skip(1)
							.SelectTry(x => CreateCatalogType(x))
							.OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null; })
							.Where(x => x != null);
		}

		private static CatalogType CreateCatalogType(String brand) {
			brand = brand.Trim('"').Trim();
			if (String.IsNullOrEmpty(brand)) {
				throw new Exception("CatalogBrand name empty");
			}
			return new CatalogType(brand);
		}

		private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes() {
			return new List<CatalogType>() {
				new CatalogType("Mug"),
				new CatalogType("T-Shirt"),
				new CatalogType("Sheet"),
				new CatalogType("USB Memory Stick")
			};
		}

		#endregion

		#region CatalogItems

		private static async Task<IEnumerable<CatalogItem>> GetCatalogItemsFromFile(String contentRootPath, ILogger<PrepareDatabase> logger) {
			//String csvFileCatalogBrandsPath = Path.Combine(contentRootPath, "SeedData", "Items.csv");
			String csvFileCatalogItemsPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogItems.csv";

			if (!File.Exists(csvFileCatalogItemsPath)) {
				return GetPreconfiguredCatalogItems().Result;
			}

			String[] csvHeaders;
			try {
				String[] requiredHeaders = { "catalogtypename", "catalogbrandname", "name", "description", "price", "picturefilename" };
				String[] optionalHeaders = { "availablestock", "restockthreshold", "maxstockthreshold", "onreorder" };
				csvHeaders = await GetHeaders(csvFileCatalogItemsPath, requiredHeaders, optionalHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogItems().Result;
			}

			Dictionary<String, int> catalogTypeIdLookup = await _unitOfWork.CatalogTypeRepository.GetDictionaryAsync();
			Dictionary<String, int> catalogBrandIdLookp = await _unitOfWork.CatalogBrandRepository.GetDictionaryAsync();

			return File.ReadAllLines(csvFileCatalogItemsPath)
					   .Skip(1)
					   .Select(rows => Regex.Split(rows, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
					   .SelectTry(column => CreateCatalogItem(column, csvHeaders, catalogTypeIdLookup, catalogBrandIdLookp).Result)
					   .OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null; })
					   .Where(x => x != null);
		}

		private static async Task<CatalogItem> CreateCatalogItem(String[] columns, String[] headers, Dictionary<String, int> catalogTypeIdLookup, Dictionary<String, int> catalogBrandIdLookup) {
			if (columns.Count() != headers.Count()) {
				throw new Exception($"Column count '{columns.Count()}' mismatches headers count '{headers.Count()}'");
			}

			String catalogTypeName = columns[Array.IndexOf(headers,"catalogtypename")].Trim('"').Trim();
			if (!catalogTypeIdLookup.ContainsKey(catalogTypeName)) {
				throw new Exception($"Type = '{catalogTypeName}' doesn't exist in CatalogTypes = '{catalogTypeIdLookup.Values}'");
			}

			String catalogBrandName = columns[Array.IndexOf(headers, "catalogbrandname")].Trim('"').Trim();
			if (!catalogBrandIdLookup.ContainsKey(catalogBrandName)) {
				throw new Exception($"Brand = '{catalogBrandName}' doesn't exist in CatalogBrands = '{catalogBrandIdLookup.Values}'");
			}

			String priceString = columns[Array.IndexOf(headers, "price")];
			decimal price;
			try {
				price = decimal.Parse(priceString);
			} catch (Exception exception) {
				throw new Exception($"Price = '{priceString}' is not a valid integer number. Exception: {exception.Message}.");
			}

			CatalogItem catalogItem = new CatalogItem(
				name: columns[Array.IndexOf(headers, "name")].Trim('"').Trim(), 
				description: columns[Array.IndexOf(headers, "description")].Trim('"').Trim(),
				catalogType: await _unitOfWork.CatalogTypeRepository.GetAsync(catalogTypeIdLookup[catalogTypeName]),
				catalogBrand: await _unitOfWork.CatalogBrandRepository.GetAsync(catalogBrandIdLookup[catalogBrandName]),
				pictureFileName: columns[Array.IndexOf(headers, "picturefilename")].Trim('"').Trim(),
				price: price
			);

			int retockThresholdIndex = Array.IndexOf(headers, "restockthreshold");
			if (retockThresholdIndex != -1) {
				String restockThresholdString = columns[retockThresholdIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(restockThresholdString)) {
					try {
						int restockThreshold = int.Parse(restockThresholdString);
						catalogItem.RestockThreshold = restockThreshold;
					} catch (Exception exception) {
						throw new Exception($"RestockThreshold = '{restockThresholdString}' isn't a valid integer number. Exception: {exception.Message}."); 
					}
				}
			}

			int maxStockTresholdIndex = Array.IndexOf(headers, "maxstockthreshold");
			if (maxStockTresholdIndex != -1) {
				String maxStockThresholdString = columns[maxStockTresholdIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(maxStockThresholdString)) {
					try {
						int maxStockThreshold = int.Parse(maxStockThresholdString);
						catalogItem.MaxStockThreshold = maxStockThreshold;
					} catch (Exception exception) {
						throw new Exception($"MaxStockThreshold = '{maxStockThresholdString}' isn't a valid integer number. Exception: {exception.Message}.");
					}
				}
			}

			int onReorderIndex = Array.IndexOf(headers, "onreorder");
			if (onReorderIndex != -1) { 
				String onReorderString = columns[onReorderIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(onReorderString)) {
					try {
						bool onReorder = bool.Parse(onReorderString);
						catalogItem.OnReorder = onReorder;
					} catch (Exception exception) {
						throw new Exception($"OnReorder = '{onReorderString}' isn't a valid boolean value. Exception: {exception.Message}.");
					}
				}
			}

			return catalogItem;
		}

		private static async Task<IEnumerable<CatalogItem>> GetPreconfiguredCatalogItems() {
			IEnumerable<CatalogBrand> brands = await _unitOfWork.CatalogBrandRepository.GetAllAsync();
			IEnumerable<CatalogType> types = await _unitOfWork.CatalogTypeRepository.GetAllAsync();
			return new List<CatalogItem>() {
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : ".NET Bot Black Hoodie", name: ".NET Bot Black Hoodie", price: 19.5M, pictureFileName: "1.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 1), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : ".NET Black & White Mug", name: ".NET Black & White Mug", price: 8.50M, pictureFileName: "2.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Prism White T-Shirt", name: "Prism White T-Shirt", price: 12, pictureFileName: "3.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : ".NET Foundation T-shirt", name: ".NET Foundation T-shirt", price: 12, pictureFileName: "4.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 3), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Roslyn Red Sheet", name: "Roslyn Red Sheet", price: 8.5M, pictureFileName: "5.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : ".NET Blue Hoodie", name: ".NET Blue Hoodie", price: 12, pictureFileName: "6.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Roslyn Red T-Shirt", name: "Roslyn Red T-Shirt", price: 12, pictureFileName: "7.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Kudu Purple Hoodie", name: "Kudu Purple Hoodie", price: 8.5M, pictureFileName: "8.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 1), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Cup<T> White Mug", name: "Cup<T> White Mug", price: 12, pictureFileName: "9.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 3), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : ".NET Foundation Sheet", name: ".NET Foundation Sheet", price: 12, pictureFileName: "10.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 3), catalogBrand: brands.Single(x => x.Id == 2), availableStock: 100, description : "Cup<T> Sheet", name: "Cup<T> Sheet", price: 8.5M, pictureFileName: "11.png"),
				new CatalogItem(catalogType: types.Single(x => x.Id == 2), catalogBrand: brands.Single(x => x.Id == 5), availableStock: 100, description : "Prism White TShirt", name: "Prism White TShirt", price: 12, pictureFileName: "12.png"),
			};
		}

		#endregion

		#region Helpers

		private static async Task<String[]> GetHeaders(String csvFile, String[] requiredHeaders, String[]? optionalHeaders = null) {
			String[] fileLines = await File.ReadAllLinesAsync(csvFile);
			String[] csvHeaders	= fileLines.First().ToLowerInvariant().Split(',');

			if (csvHeaders.Count() < requiredHeaders.Count()) {
				throw new Exception($"Required header count '{requiredHeaders.Count()}' is greater than CSV header count'{csvHeaders.Count()}'");
			}

			if (optionalHeaders != null && (csvHeaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))) {
				throw new Exception($"CSV header count {csvHeaders.Count()} is larger than required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
			}

			foreach (String requiredHeader in requiredHeaders) {
				if (!csvHeaders.Contains(requiredHeader)) {
					throw new Exception($"CSV doesn't contain  required header '{requiredHeader}'");
				}
			}

			return csvHeaders;
		}

		private static AsyncRetryPolicy CreatePolicy(ILogger<PrepareDatabase> logger, string prefix, int retries = 3) {
			return Policy.Handle<SqlException>().WaitAndRetryAsync(
				retryCount: retries,
				sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
				onRetry: (exception, timeSpan, retry, Context) => logger.LogWarning(exception, $"[{prefix} Exception {exception.GetType().Name} with message {exception.Message} detected on attempt {retry} of {retries}]")
			);
		}

		#endregion
	}
}
