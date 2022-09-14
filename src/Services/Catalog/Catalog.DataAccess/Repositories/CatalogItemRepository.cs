using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository {
		private readonly CatalogContext _catalogContext;

		public CatalogItemRepository(CatalogContext catalogContext) : base(catalogContext) { 
			this._catalogContext = catalogContext;
		}
	}
}