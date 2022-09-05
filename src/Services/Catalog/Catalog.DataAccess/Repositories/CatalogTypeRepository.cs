using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public class CatalogTypeRepository : Repository<CatalogType>, ICatalogTypeRepository {

		private readonly CatalogContext _catalogContext;

		public CatalogTypeRepository(CatalogContext catalogContext) : base(catalogContext) {
			_catalogContext = catalogContext;
		}
	}
}