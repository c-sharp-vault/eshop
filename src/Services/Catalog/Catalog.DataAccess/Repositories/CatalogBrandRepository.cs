using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {
	public class CatalogBrandRepository : Repository<CatalogBrand>, ICatalogBrandRepository {
		private readonly CatalogContext _catalogContext;

		public CatalogBrandRepository(CatalogContext catalogContext) : base(catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<Dictionary<string, int>> GetDictionaryAsync() {
			return await _catalogContext.CatalogBrands.ToDictionaryAsync(x => x.Brand, x => x.CatalogBrandID);
		}
    }
}