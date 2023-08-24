using Catalog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Repositories {
	public interface ICatalogItemRepository : IRepository<CatalogItem> {
		new Task<CatalogItem> CreateAsync(CatalogItem catalogItem);
		Task<IReadOnlyCollection<CatalogItem>> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested);
		new Task<CatalogItem> GetSingleAsync(int id);
		Task<bool> NameExistsAsync(string name);
		Task<CatalogItem> UpdateAsync(CatalogItem catalogItemUpdateDTO);
	}
}