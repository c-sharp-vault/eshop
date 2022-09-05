using Catalog.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DataAccess {
	public class CatalogContextSeed : ICatalogContextSeed {
		public Task SeedAsync(CatalogContext catalogContext, IWebHostEnvironment webHostEnvironment,IOptions<CatalogOptions> settings, ILogger<CatalogContextSeed> logger) {
			throw new NotImplementedException();
		}
	}
}
