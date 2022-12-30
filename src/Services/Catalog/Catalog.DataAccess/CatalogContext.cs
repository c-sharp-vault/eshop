using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess {
	public class CatalogContext : DbContext, ICatalogContext {

		public CatalogContext() { }

		public CatalogContext(DbContextOptions<CatalogContext> dbContextOptions) : base(dbContextOptions) { }

		public DbSet<CatalogItem> CatalogItems { get; set; }

		public DbSet<CatalogBrand> CatalogBrands { get; set; }

		public DbSet<CatalogType> CatalogTypes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options) {
			if (!options.IsConfigured) options.UseNpgsql();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) =>
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
	}
}