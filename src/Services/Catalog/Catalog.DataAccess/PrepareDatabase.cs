using Catalog.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Data.SqlClient;
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

					await _unitOfWork.DisposeAsync();
				});
			}
		}

		#region Helpers

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
