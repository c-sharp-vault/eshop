using Catalog.DataAccess.Repositories;

namespace Catalog.DataAccess {
	public interface IUnitOfWork {
		public ICatalogItemRepository CatalogItemRepository { get; }
		bool Complete();
	}
}