using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Repositories {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityType {

		private readonly CatalogContext _catalogContext;

		public Repository(CatalogContext catalogContext) {
			this._catalogContext = catalogContext;
		}

		public async Task<TEntity> GetAsync(int id) {
			if (id == 0) throw new ArgumentOutOfRangeException(nameof(id), "Must be greater than zero");
			if (!ExistsAsync(id).Result) throw new RecordNotFoundException($"{nameof(TEntity)} with Id = {id} doesn't exist");
			return await _catalogContext.Set<TEntity>().FindAsync(id);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync() => await _catalogContext.Set<TEntity>().ToListAsync();

		public async Task<bool> AnyAsync() => (await _catalogContext.Set<TEntity>().AnyAsync());

		public async Task<bool> ExistsAsync(int id) => (await _catalogContext.Set<TEntity>().FindAsync(id) != null);

		public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null) => 
			await _catalogContext.Set<TEntity>().Where(predicate).ToListAsync();

		public async Task AddAsync(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _catalogContext.Set<TEntity>().AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			await _catalogContext.Set<TEntity>().AddRangeAsync(entities);
		}

		public void Update(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_catalogContext.Set<TEntity>().Update(entity);
		}

		public void UpdateRange(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			_catalogContext.Set<TEntity>().UpdateRange(entities);
		}

		public async Task RemoveAsync(int id) {
			if (id == 0) throw new ArgumentOutOfRangeException(nameof(id), "Must be greater than zero");
			if (!ExistsAsync(id).Result) throw new RecordNotFoundException($"{nameof(TEntity)} with ID = {id} doesn't exist");

			TEntity? entity = await _catalogContext.Set<TEntity>().FindAsync(id);
			if (entity == null) throw new RecordNotFoundException($"{nameof(TEntity)} with ID = {id} doesn't exist");
			
			_catalogContext.Remove(entity);
		}

		public async Task RemoveRangeAsync(IEnumerable<TEntity> entities) {
			throw new NotImplementedException();
		}
	}
}
