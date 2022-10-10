using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.DataAccess.Repositories {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityType {

		private readonly CatalogContext _catalogContext;

		public Repository(CatalogContext catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<TEntity> GetAsync(int id) {
			if (id == 0) throw new ArgumentNullException(nameof(id));
			if (!ExistsAsync(id).Result) throw new RecordNotFoundException($"{nameof(TEntity)} with Id = {id} doesn't exist");
			return await _catalogContext.Set<TEntity>().FindAsync(id);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync() {
			return await _catalogContext.Set<TEntity>().ToListAsync();
		}

		public async Task<bool> AnyAsync() {
			return (await _catalogContext.Set<TEntity>().AnyAsync());
		}

		public async Task<bool> ExistsAsync(int id) {
			return (await _catalogContext.Set<TEntity>().FindAsync(id) != null);
		}

		public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null) {
			var res = await _catalogContext.Set<TEntity>().Where(predicate).ToListAsync();
			return res;
		}

		public async Task AddAsync(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _catalogContext.Set<TEntity>().AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			await _catalogContext.Set<TEntity>().AddRangeAsync(entities);
		}

		public async Task RemoveAsync(TEntity entity) {
			throw new NotImplementedException();
		}

		public async Task RemoveRangeAsync(IEnumerable<TEntity> entities) {
			throw new NotImplementedException();
		}
	}
}
