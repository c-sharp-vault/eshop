using System;
using System.Threading.Tasks;
using Catalog.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccess {
	public class UnitOfWork : IUnitOfWork {
		private readonly CatalogDbContext _catalogContext;
		private readonly ICatalogItemRepository _catalogItemRepository;
		private readonly ICatalogBrandRepository _catalogBrandRepository;
		private readonly ICatalogTypeRepository _catalogTypeRepository;

		public UnitOfWork(
			CatalogDbContext catalogContext, 
			ICatalogItemRepository catalogItemRepository, 
			ICatalogBrandRepository catalogBrandRepository, 
			ICatalogTypeRepository catalogTypeRepository) {
			_catalogContext = catalogContext;
			_catalogItemRepository = catalogItemRepository;
			_catalogBrandRepository = catalogBrandRepository;
			_catalogTypeRepository = catalogTypeRepository;
		}	

		public ICatalogItemRepository CatalogItemRepository => _catalogItemRepository;

		public ICatalogBrandRepository CatalogBrandRepository => _catalogBrandRepository;

		public ICatalogTypeRepository CatalogTypeRepository => _catalogTypeRepository;

		public bool Complete() => _catalogContext.SaveChanges() > 0;

		public async Task<bool> CompleteAsync() => await _catalogContext.SaveChangesAsync() > 0;

		public async Task MigrateAsync() {
			await _catalogContext.Database.MigrateAsync();
		}

		public async Task DisposeAsync() {
			await DisposeAsync(true);
			GC.SuppressFinalize(this);
		}

		protected virtual async Task DisposeAsync(bool disposing) {
			if (disposing) {
				await _catalogContext.DisposeAsync();
			}
		}
	}
}
