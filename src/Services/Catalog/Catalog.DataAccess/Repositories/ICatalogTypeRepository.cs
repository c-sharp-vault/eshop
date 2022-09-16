using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogTypeRepository : IRepository<CatalogType> {
		Task<Dictionary<String, int>> GetDictionaryAsync();
	}
}
