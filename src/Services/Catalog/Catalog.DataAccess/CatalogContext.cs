using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess {
	public class CatalogContext : DbContext, ICatalogContext {

		private DbSet<CatalogItem> _catalogItems;
		private DbSet<CatalogBrand> _catalogBrands;
		private DbSet<CatalogType> _catalogTypes;

		public CatalogContext(DbContextOptions<CatalogContext> dbContextOptions) : base(dbContextOptions) { }

		public DbSet<CatalogItem> CatalogItems {
			get { return _catalogItems; }
			set { _catalogItems = value; }
		}

		public DbSet<CatalogBrand> CatalogBrands {
			get { return _catalogBrands; }
			set { _catalogBrands = value; }
		}

		public DbSet<CatalogType> CatalogTypes {
			get { return _catalogTypes; }
			set { _catalogTypes = value; }
		}
	}
}