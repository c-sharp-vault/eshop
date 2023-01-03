using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Catalog.Infrastructure.Options;
using System.Threading.Tasks;

namespace Catalog.DataAccess {
	public interface IPrepareDatabase {
		Task SeedAsync(CatalogDbContext catalogContext, IWebHostEnvironment webHostEnvironment, IOptions<CatalogOptions> settings, ILogger<PrepareDatabase> logger);
	}
}
