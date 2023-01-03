
using Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Catalog.IntegrationTests.Initialization {
	internal class TestHelpers {
		public static IConfiguration GetConfiguration() =>
			new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.testing.json", true, true)
					.Build();

		public static CatalogDbContext GetContext(IConfiguration configuration) {
			var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
			var connectionString = configuration.GetConnectionString("PostgreSQLCatalogConnectionstring");
			optionsBuilder.UseNpgsql(connectionString, npgSqlDbContextOptionsBuilder => {
				npgSqlDbContextOptionsBuilder.MigrationsAssembly("Catalog.DataAccess");
			});
			var dbContext = new CatalogDbContext(optionsBuilder.Options);
			dbContext.Database.Migrate();
			return dbContext;
		}
	}
}
