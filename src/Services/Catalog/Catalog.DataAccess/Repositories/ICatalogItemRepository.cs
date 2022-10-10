using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogItemRepository : IRepository<CatalogItem> {
		Task<IEnumerable<CatalogItem>> GetAllAsync(byte pageSize, byte pageIndex);

		Task<bool> NameExistsAsync(CatalogItem catalogItem);
	}
}