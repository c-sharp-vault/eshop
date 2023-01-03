using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {
	public class CatalogTypeRepository : Repository<CatalogType>, ICatalogTypeRepository {
		private readonly CatalogDbContext _catalogContext;

		public CatalogTypeRepository(CatalogDbContext catalogContext) : base(catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<Dictionary<string, int>> GetDictionaryAsync() {
			return await _catalogContext.CatalogTypes.ToDictionaryAsync(x => x.Type, x => x.CatalogTypeID);
		}
	}
}