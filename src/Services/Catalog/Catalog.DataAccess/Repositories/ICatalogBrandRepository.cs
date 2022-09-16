﻿using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogBrandRepository : IRepository<CatalogBrand> {
		Task<Dictionary<String, int>> GetDictionaryAsync();
	}
}