using System.Threading.Tasks;
using Catalog.DataAccess.Repositories;

namespace Catalog.DataAccess {
	public interface IUnitOfWork {
		ICatalogItemRepository CatalogItemRepository { get; }
		ICatalogBrandRepository CatalogBrandRepository { get; }
		ICatalogTypeRepository CatalogTypeRepository { get; }
		Task MigrateAsync();
		void Complete();
		Task CompleteAsync();
		Task DisposeAsync();
	}
}