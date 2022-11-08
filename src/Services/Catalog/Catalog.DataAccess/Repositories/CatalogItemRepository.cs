using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {
	public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository {
		private readonly CatalogContext _catalogContext;

		public CatalogItemRepository(CatalogContext catalogContext) : base(catalogContext) { 
			this._catalogContext = catalogContext;
		}

		public async Task<bool> NameExistsAsync(CatalogItem catalogItem) {
			return await _catalogContext.CatalogItems.AnyAsync(x => x.Name == catalogItem.Name);
		}

		public async Task<CatalogItem> GetAsync(int id) {
			if (id == 0) {
				throw new ArgumentNullException(nameof(id));
			}

			if (!ExistsAsync(id).Result) {
				throw new RecordNotFoundException($"{nameof(CatalogItem)} with Id = {id} doesn't exist");
			}

			CatalogItem catalogItem = await _catalogContext.CatalogItems.FindAsync(id);
			await _catalogContext.Entry(catalogItem).Reference(x => x.CatalogBrand).LoadAsync();
			await _catalogContext.Entry(catalogItem).Reference(x => x.CatalogType).LoadAsync();
			return catalogItem;
		}

		public async Task<IEnumerable<CatalogItem>> GetAllAsync() {
			return await _catalogContext.CatalogItems.Include(x => x.CatalogBrand).Include(x => x.CatalogType).ToListAsync();
		}

		public async Task<IEnumerable<CatalogItem>> GetAllAsync(byte pageSize, byte pageIndex) {
			return await _catalogContext.CatalogItems
											.OrderBy(x => x.Id)
											.Skip(pageSize * pageIndex)
											.Take(pageSize)
											.Include(x => x.CatalogBrand)
											.Include(x => x.CatalogType)
											.ToListAsync();
		}
	}
}