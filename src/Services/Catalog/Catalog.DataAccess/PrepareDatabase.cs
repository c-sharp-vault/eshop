using Catalog.Core.Models;
using Catalog.Infrastructure.Extensions.Linq;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
				string contentRootPath = _webHostEnvironment.ContentRootPath;
				string picturePath = _webHostEnvironment.WebRootPath;

				AsyncRetryPolicy policy = CreatePolicy(_logger, nameof(PrepareDatabase));

				await policy.ExecuteAsync(async () => {

					_logger.LogInformation("--> Applying Migrations...");
					await _unitOfWork.MigrateAsync();

					if (((WebApplication)applicationBuilder).Environment.IsDevelopment()) {
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
					}

					await _unitOfWork.DisposeAsync();
				});
			}
		}

		#region CatalogBrands

		private static async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsFromFile(string contentRootPath, ILogger<PrepareDatabase> logger) {
			//string csvFileCatalogBrandsPath = Path.Combine(contentRootPath, "SeedData", "Brands.csv");
			string csvFileCatalogBrandsPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogBrands.csv";

			if (!File.Exists(csvFileCatalogBrandsPath)) {
				return GetPreconfiguredCatalogBrands();
			}

			string[] csvHeaders;
			try {
				string[] requiredHeaders = { "catalogbrand" };
				csvHeaders = await GetHeaders(csvFileCatalogBrandsPath, requiredHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogBrands();
			}

			string[] fileLines = await File.ReadAllLinesAsync(csvFileCatalogBrandsPath);
			return fileLines.Skip(1)
							.SelectTry(x => CreateCatalogBrand(x))
							.OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null;})
							.Where(x => x != null);
		}

		private static CatalogBrand CreateCatalogBrand(string brand) {
			brand = brand.Trim('"').Trim();
			if (string.IsNullOrEmpty(brand)) {
				throw new Exception("CatalogBrand name empty");
			}
			return new CatalogBrand() { Brand = brand };
		}

		private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands() {
			return new List<CatalogBrand>() {
				new CatalogBrand() { Brand = "Azure" },
				new CatalogBrand() { Brand = ".NET" },
				new CatalogBrand() { Brand = "Visual Studio" },
				new CatalogBrand() { Brand = "SQL Server" },
				new CatalogBrand() { Brand = "Other" },
			};
		}

		#endregion

		#region CatalogTypes

		private static async Task<IEnumerable<CatalogType>> GetCatalogTypesFromFile(string contentRootPath, ILogger<PrepareDatabase> logger) {
			//string csvFileCatalogTypesPath = Path.Combine(contentRootPath, "SeedData", "Types.csv");
			string csvFileCatalogTypesPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogTypes.csv";

			if (!File.Exists(csvFileCatalogTypesPath)) {
				return GetPreconfiguredCatalogTypes();
			}

			string[] csvHeaders;
			try {
				string[] requiredHeaders = { "catalogtype" };
				csvHeaders = await GetHeaders(csvFileCatalogTypesPath, requiredHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogTypes();
			}

			string[] fileLines = await File.ReadAllLinesAsync(csvFileCatalogTypesPath);
			return fileLines.Skip(1)
							.SelectTry(x => CreateCatalogType(x))
							.OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null; })
							.Where(x => x != null);
		}

		private static CatalogType CreateCatalogType(string type) {
			type = type.Trim('"').Trim();
			if (string.IsNullOrEmpty(type)) {
				throw new Exception("CatalogBrand name empty");
			}
			return new CatalogType() { Type = type };
		}

		private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes() {
			return new List<CatalogType>() {
				new CatalogType() { Type = "Mug" },
				new CatalogType() { Type = "T-Shirt" },
				new CatalogType() { Type = "Sheet" },
				new CatalogType() { Type = "USB Memory Stick" }
			};
		}

		#endregion

		#region CatalogItems

		private static async Task<IEnumerable<CatalogItem>> GetCatalogItemsFromFile(string contentRootPath, ILogger<PrepareDatabase> logger) {
			//string csvFileCatalogBrandsPath = Path.Combine(contentRootPath, "SeedData", "Items.csv");
			string csvFileCatalogItemsPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\CatalogItems.csv";

			if (!File.Exists(csvFileCatalogItemsPath)) {
				return GetPreconfiguredCatalogItems().Result;
			}

			string[] csvHeaders;
			try {
				string[] requiredHeaders = { "catalogtypename", "catalogbrandname", "name", "description", "price", "picturefilename" };
				string[] optionalHeaders = { "availablestock", "restockthreshold", "maxstockthreshold", "onreorder" };
				csvHeaders = await GetHeaders(csvFileCatalogItemsPath, requiredHeaders, optionalHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogItems().Result;
			}

			Dictionary<string, int> catalogTypeIdLookup = await _unitOfWork.CatalogTypeRepository.GetDictionaryAsync();
			Dictionary<string, int> catalogBrandIdLookp = await _unitOfWork.CatalogBrandRepository.GetDictionaryAsync();

			return File.ReadAllLines(csvFileCatalogItemsPath)
					   .Skip(1)
					   .Select(rows => Regex.Split(rows, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
					   .SelectTry(column => CreateCatalogItem(column, csvHeaders, catalogTypeIdLookup, catalogBrandIdLookp).Result)
					   .OnCaughtException(x => { logger.LogError(x, $"EXCEPTION ERROR: {x.Message}"); return null; })
					   .Where(x => x != null);
		}

		private static async Task<CatalogItem> CreateCatalogItem(string[] columns, string[] headers, Dictionary<string, int> catalogTypeIdLookup, Dictionary<string, int> catalogBrandIdLookup) {
			if (columns.Count() != headers.Count()) {
				throw new Exception($"Column count '{columns.Count()}' mismatches headers count '{headers.Count()}'");
			}

			string catalogTypeName = columns[Array.IndexOf(headers,"catalogtypename")].Trim('"').Trim();
			if (!catalogTypeIdLookup.ContainsKey(catalogTypeName)) {
				throw new Exception($"Type = '{catalogTypeName}' doesn't exist in CatalogTypes = '{catalogTypeIdLookup.Values}'");
			}

			string catalogBrandName = columns[Array.IndexOf(headers, "catalogbrandname")].Trim('"').Trim();
			if (!catalogBrandIdLookup.ContainsKey(catalogBrandName)) {
				throw new Exception($"Brand = '{catalogBrandName}' doesn't exist in CatalogBrands = '{catalogBrandIdLookup.Values}'");
			}

			string pricestring = columns[Array.IndexOf(headers, "price")];
			decimal price;
			try {
				price = decimal.Parse(pricestring);
			} catch (Exception exception) {
				throw new Exception($"Price = '{pricestring}' is not a valid integer number. Exception: {exception.Message}.");
			}

			CatalogItem catalogItem = new CatalogItem() {
				Name = columns[Array.IndexOf(headers, "name")].Trim('"').Trim(),
				Description = columns[Array.IndexOf(headers, "description")].Trim('"').Trim(),
				CatalogType = await _unitOfWork.CatalogTypeRepository.GetAsync(catalogTypeIdLookup[catalogTypeName]),
				CatalogBrand = await _unitOfWork.CatalogBrandRepository.GetAsync(catalogBrandIdLookup[catalogBrandName]),
				PictureFileName = columns[Array.IndexOf(headers, "picturefilename")].Trim('"').Trim(),
				Price = price
			};

			int retockThresholdIndex = Array.IndexOf(headers, "restockthreshold");
			if (retockThresholdIndex != -1) {
				string restockThresholdstring = columns[retockThresholdIndex].Trim('"').Trim();
				if (!string.IsNullOrEmpty(restockThresholdstring)) {
					try {
						int restockThreshold = int.Parse(restockThresholdstring);
						catalogItem.RestockThreshold = restockThreshold;
					} catch (Exception exception) {
						throw new Exception($"RestockThreshold = '{restockThresholdstring}' isn't a valid integer number. Exception: {exception.Message}."); 
					}
				}
			}

			int maxStockTresholdIndex = Array.IndexOf(headers, "maxstockthreshold");
			if (maxStockTresholdIndex != -1) {
				string maxStockThresholdstring = columns[maxStockTresholdIndex].Trim('"').Trim();
				if (!string.IsNullOrEmpty(maxStockThresholdstring)) {
					try {
						int maxStockThreshold = int.Parse(maxStockThresholdstring);
						catalogItem.MaxStockThreshold = maxStockThreshold;
					} catch (Exception exception) {
						throw new Exception($"MaxStockThreshold = '{maxStockThresholdstring}' isn't a valid integer number. Exception: {exception.Message}.");
					}
				}
			}

			int onReorderIndex = Array.IndexOf(headers, "onreorder");
			if (onReorderIndex != -1) { 
				string onReorderstring = columns[onReorderIndex].Trim('"').Trim();
				if (!string.IsNullOrEmpty(onReorderstring)) {
					try {
						bool onReorder = bool.Parse(onReorderstring);
						catalogItem.OnReorder = onReorder;
					} catch (Exception exception) {
						throw new Exception($"OnReorder = '{onReorderstring}' isn't a valid boolean value. Exception: {exception.Message}.");
					}
				}
			}

			return catalogItem;
		}

		private static async Task<IEnumerable<CatalogItem>> GetPreconfiguredCatalogItems() {
			IEnumerable<CatalogBrand> brands = await _unitOfWork.CatalogBrandRepository.GetAllAsync();
			IEnumerable<CatalogType> types = await _unitOfWork.CatalogTypeRepository.GetAllAsync();
			return new List<CatalogItem>() {
				new CatalogItem() {
					Name = ".NET Bot Black Hoodie",
					Description = ".NET Bot Black Hoodie",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 19.5m,
					PictureFileName = "1.png"
				},
				new CatalogItem() {
					Name = ".NET Black & White Mug",
					Description = ".NET Black & White Mug",
					CatalogType = types.Single(x => x.ID == 1),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 8.50m,
					PictureFileName = "2.png"
				},
				new CatalogItem() {
					Name = "Prism White T-Shirt",
					Description = "Prism White T-Shirt",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "3.png"
				},
				new CatalogItem() {
					Name = ".NET Foundation T-shirt",
					Description = ".NET Foundation T-shirt",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "4.png"
				},
				new CatalogItem() {
					Name = "Roslyn Red Sheet",
					Description = "Roslyn Red Sheet",
					CatalogType = types.Single(x => x.ID == 3),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 8.5m,
					PictureFileName = "5.png"
				},
				new CatalogItem() {
					Name = ".NET Blue Hoodie",
					Description = ".NET Blue Hoodie",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "6.png"
				},
				new CatalogItem() {
					Name = "Roslyn Red T-Shirt",
					Description = "Roslyn Red T-Shirt",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "7.png"
				},
				new CatalogItem() {
					Name = "Kudu Purple Hoodie",
					Description = "Kudu Purple Hoodie",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 8.5m,
					PictureFileName = "8.png"
				},
				new CatalogItem() {
					Name = "Cup<T> White Mug",
					Description = "Cup<T> White Mug",
					CatalogType = types.Single(x => x.ID == 1),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "9.png"
				},
				new CatalogItem() {
					Name = ".NET Foundation Sheet",
					Description = ".NET Foundation Sheet",
					CatalogType = types.Single(x => x.ID == 3),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "10.png"
				},
				new CatalogItem() {
					Name = "Cup<T> Sheet",
					Description = "Cup<T> Sheet",
					CatalogType = types.Single(x => x.ID == 3),
					CatalogBrand = brands.Single(x => x.ID == 2),
					AvailableStock = 100,
					Price = 8.5m,
					PictureFileName = "11.png"
				},
				new CatalogItem() {
					Name = "Prism White TShirt",
					Description = "Prism White TShirt",
					CatalogType = types.Single(x => x.ID == 2),
					CatalogBrand = brands.Single(x => x.ID == 5),
					AvailableStock = 100,
					Price = 12.0m,
					PictureFileName = "12.png"
				}
			};
		}

		#endregion

		#region Helpers

		private static async Task<string[]> GetHeaders(string csvFile, string[] requiredHeaders, string[]? optionalHeaders = null) {
			string[] fileLines = await File.ReadAllLinesAsync(csvFile);
			string[] csvHeaders	= fileLines.First().ToLowerInvariant().Split(',');

			if (csvHeaders.Count() < requiredHeaders.Count()) {
				throw new Exception($"Required header count '{requiredHeaders.Count()}' is greater than CSV header count'{csvHeaders.Count()}'");
			}

			if (optionalHeaders != null && (csvHeaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))) {
				throw new Exception($"CSV header count {csvHeaders.Count()} is larger than required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
			}

			foreach (string requiredHeader in requiredHeaders) {
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
