using Catalog.Core.Models;
using Catalog.Infrastructure.Extensions.Linq;
using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Catalog.DataAccess {
	public class PrepareDatabase {
		public static async Task MigrateAndSeedAsync(IApplicationBuilder applicationBuilder) {
			using (IServiceScope serviceScope = applicationBuilder.ApplicationServices.CreateScope()) {

				IUnitOfWork unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
				ILogger<PrepareDatabase> logger = serviceScope.ServiceProvider.GetService<ILogger<PrepareDatabase>>();
				IOptions<CatalogOptions> options = serviceScope.ServiceProvider.GetService<IOptions<CatalogOptions>>();
				IWebHostEnvironment webHostEnvironment = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

				bool useCustomizationData = options.Value.UseCustomizationData;
				String contentRootPath = webHostEnvironment.ContentRootPath;
				String picturePath = webHostEnvironment.WebRootPath;

				AsyncRetryPolicy policy = CreatePolicy(logger, nameof(PrepareDatabase));

				await policy.ExecuteAsync(async () => {

					logger.LogInformation("--> Applying Migrations...");
					await unitOfWork.MigrateAsync();

					if (!unitOfWork.CatalogBrandRepository.AnyAsync().Result) {
						logger.LogInformation("--> Seeding...");
						await unitOfWork.CatalogBrandRepository.AddRangeAsync(
							useCustomizationData
							? GetCatalogBrandsFromFile(contentRootPath, logger)
							: GetPreconfiguredCatalogBrands()
						);
						await unitOfWork.CompleteAsync();
					}
				});
			}
		}

		private static AsyncRetryPolicy CreatePolicy(ILogger<PrepareDatabase> logger, string prefix, int retries = 3) {
			return Policy.Handle<SqlException>().WaitAndRetryAsync(
				retryCount: retries,
				sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
				onRetry: (exception, timeSpan, retry, Context) => logger.LogWarning(exception, $"[{prefix} Exception {exception.GetType().Name} with message {exception.Message} detected on attempt {retry} of {retries}]")
			);
		}

		private static IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(String contentRootPath, ILogger<PrepareDatabase> logger) {
			//String csvFileCatalogBrandsPath = Path.Combine(contentRootPath, "SeedData", "Brands.csv");
			String csvFileCatalogBrandsPath = @"C:\Users\Fedex\source\repos\eShop\src\Services\Catalog\Catalog.DataAccess\SeedData\Brands.csv";

			if (!File.Exists(csvFileCatalogBrandsPath)) {
				return GetPreconfiguredCatalogBrands();
			}

			String[] csvHeaders;
			try {
				String[] requiredHeaders = { "brand" };
				csvHeaders = GetHeaders(csvFileCatalogBrandsPath, requiredHeaders);
			} catch (Exception exception) {
				logger.LogError(exception, $"EXCEPTION ERROR: {exception.Message}");
				return GetPreconfiguredCatalogBrands();
			}

			return File.ReadAllLines(csvFileCatalogBrandsPath)
					   .Skip(1)
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
				new CatalogBrand("Coca-Cola"),
				new CatalogBrand("Quilmes"),
				new CatalogBrand("Terrabusi")
			};
		}

		private static String[] GetHeaders(String csvFile, String[] requiredHeaders, String[] optionalHeaders = null) {
			String[] csvHeaders = File.ReadLines(csvFile).First().ToLowerInvariant().Split(',');

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
	}
}
