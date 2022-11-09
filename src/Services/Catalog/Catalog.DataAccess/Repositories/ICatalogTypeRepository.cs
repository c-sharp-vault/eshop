using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogTypeRepository : IRepository<CatalogType> {
		Task<Dictionary<string, int>> GetDictionaryAsync();
	}
}
