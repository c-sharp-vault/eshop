using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Catalog.API;

namespace Catalog.DataAccess {
	public interface ICatalogContextSeed {
		Task SeedAsync(CatalogContext catalogContext, IWebHostEnvironment webHostEnvironment, IOptions<CatalogOptions> settings, ILogger<CatalogContextSeed> logger);
	}
}
