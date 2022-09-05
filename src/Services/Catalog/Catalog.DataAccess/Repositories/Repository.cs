using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.DataAccess.Repositories {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity {

		private readonly CatalogContext _catalogContext;

		public Repository(CatalogContext catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<TEntity> Get(int id) {
			if (id == 0) throw new ArgumentNullException(nameof(id));
			if (!Exists(id).Result) throw new RecordNotFoundException($"{nameof(TEntity)} with Id = {id} doesn't exist");
			return await _catalogContext.Set<TEntity>().FindAsync(id);
		}

		public async Task<IEnumerable<TEntity>> GetAll() {
			return await _catalogContext.Set<TEntity>().ToListAsync();
		}

		public async Task<bool> Exists(int id) {
			return (await _catalogContext.Set<TEntity>().FindAsync(id) != null);
		}

		public async Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate) {
			return await _catalogContext.Set<TEntity>().Where(predicate).ToListAsync();
		}

		public async void Add(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _catalogContext.Set<TEntity>().AddAsync(entity);
		}

		public async void AddRange(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			await _catalogContext.Set<TEntity>().AddRangeAsync(entities);
		}

		public async void Remove(TEntity entity) {
			throw new NotImplementedException();
		}

		public async void RemoveRange(IEnumerable<TEntity> entities) {
			throw new NotImplementedException();
		}
	}
}
