using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogBrandRepository : IRepository<CatalogBrand> {
		Task<Dictionary<string, int>> GetDictionaryAsync();
	}
}