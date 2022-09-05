using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public class CatalogBrandRepository : Repository<CatalogBrand>, ICatalogBrandRepository {
		
		private readonly CatalogContext _catalogContext;

		public CatalogBrandRepository(CatalogContext catalogContext) : base(catalogContext) {
			this._catalogContext = catalogContext;
		}
	}
}