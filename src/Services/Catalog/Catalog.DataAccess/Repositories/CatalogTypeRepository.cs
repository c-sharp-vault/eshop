﻿using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess.Repositories {
	public class CatalogTypeRepository : Repository<CatalogType>, ICatalogTypeRepository {
		private readonly CatalogContext _catalogContext;

		public CatalogTypeRepository(CatalogContext catalogContext) : base(catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<Dictionary<String, int>> GetDictionaryAsync() {
			return await _catalogContext.CatalogTypes.ToDictionaryAsync(x => x.Type, x => x.Id);
		}
	}
}