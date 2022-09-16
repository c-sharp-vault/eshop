using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {
	public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository {
		private readonly CatalogContext _catalogContext;

		public CatalogItemRepository(CatalogContext catalogContext) : base(catalogContext) { 
			this._catalogContext = catalogContext;
		}

		public async Task<IEnumerable<CatalogItem>> GetAllAsync() {
			return await _catalogContext.CatalogItems.Include(x => x.CatalogBrand).Include(x => x.CatalogType).ToListAsync();
		}

		public async Task<IEnumerable<CatalogItem>> GetAllAsync(byte pageSize, byte pageIndex) {
			return await _catalogContext.CatalogItems.OrderBy(x => x.Name).Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();
		}
	}
}