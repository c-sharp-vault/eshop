using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess {
	public class CatalogDbContext : DbContext, ICatalogContext {

		public CatalogDbContext() { }

		public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions) : base(dbContextOptions) { }

		public DbSet<CatalogItem> CatalogItems { get; set; }

		public DbSet<CatalogBrand> CatalogBrands { get; set; }

		public DbSet<CatalogType> CatalogTypes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			//if (!options.IsConfigured) options.UseNpgsql();
			if (!options.IsConfigured) options.UseSqlServer();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) =>
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
	}
}