using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface IRepository<TEntity> where TEntity : class, IEntity {

		// Read
		Task<TEntity> GetSingleAsync(int id);
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<bool> AnyAsync();
		Task<bool> ExistsAsync(int id);
		Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

		// Create
		Task<TEntity> CreateAsync(TEntity entity);
		Task AddRangeAsync(IEnumerable<TEntity> entities);

		// Update
		void Update(TEntity entity);
		void UpdateRange(IEnumerable<TEntity> entities);

		// Delete
		Task RemoveAsync(int id);
		Task RemoveRangeAsync(IEnumerable<TEntity> entities);
	}
}
