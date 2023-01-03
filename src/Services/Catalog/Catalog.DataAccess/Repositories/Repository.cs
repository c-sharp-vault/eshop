using Catalog.Core.Models;
using Catalog.DataAccess.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Repositories {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity {

		private readonly CatalogDbContext _catalogContext;

		public Repository(CatalogDbContext catalogContext) {
			_catalogContext = catalogContext;
		}

		public virtual async Task<TEntity> GetByIDAsync(int id) {
			if (id == 0) 
				throw new ArgumentOutOfRangeException(nameof(id), "Must be greater than zero");

			if (!ExistsAsync(id).Result)
				throw new RecordNotFoundException($"{nameof(TEntity)} entity with ID = {id} was not found.");

			return await _catalogContext.Set<TEntity>().FindAsync(id);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _catalogContext.Set<TEntity>().ToListAsync();

		public virtual async Task<bool> AnyAsync() => (await _catalogContext.Set<TEntity>().AnyAsync());

		public virtual async Task<bool> ExistsAsync(int id) => (await _catalogContext.Set<TEntity>().FindAsync(id) != null);

		public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null) => 
			await _catalogContext.Set<TEntity>().Where(predicate).ToListAsync();

		public virtual async Task<TEntity> CreateAsync(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _catalogContext.Set<TEntity>().AddAsync(entity);
			return entity;
		}

		public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			await _catalogContext.Set<TEntity>().AddRangeAsync(entities);
		}

		public virtual void Update(TEntity entity) {
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_catalogContext.Set<TEntity>().Update(entity);
		}

		public virtual void UpdateRange(IEnumerable<TEntity> entities) {
			if (!entities.Any()) throw new ArgumentException(nameof(entities));
			_catalogContext.Set<TEntity>().UpdateRange(entities);
		}

		public virtual async Task RemoveAsync(int id) {
			if (id == 0)
				throw new ArgumentOutOfRangeException(nameof(id), "The provided ID must be a positive integer.");

			TEntity entity = await _catalogContext.Set<TEntity>().FindAsync(id);
			if (entity == null) 
				throw new RecordNotFoundException($"{nameof(TEntity)} entity with ID = {id} was not found.");
			
			_catalogContext.Remove(entity);

			if (await _catalogContext.SaveChangesAsync() == 0)
				throw new DbUpdateException($"Failed trying to save deleted {nameof(TEntity)} entity from the database.");
		}

		public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities) {
			 await Task.Delay(10);
		}
	}
}
