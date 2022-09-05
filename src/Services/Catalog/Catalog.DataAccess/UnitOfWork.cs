using Catalog.DataAccess.Repositories;

namespace Catalog.DataAccess {
	public class UnitOfWork : IUnitOfWork {

		private readonly CatalogContext _catalogContext;
		private readonly ICatalogItemRepository _catalogItemRepository;

		public UnitOfWork(CatalogContext catalogContext, CatalogItemRepository catalogItemRepository) {
			_catalogContext = catalogContext;
			_catalogItemRepository = catalogItemRepository;
		}	

		public ICatalogItemRepository CatalogItemRepository { 
			get { return _catalogItemRepository; }
		}

		public bool Complete() {
			return (this._catalogContext.SaveChanges() >= 0);
		}
	}
}
