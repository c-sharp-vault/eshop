using System.Linq.Expressions;
using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface IRepository<TEntity> where TEntity : class, IEntity {

		// Read
		Task<TEntity> GetAsync(int id);
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<bool> AnyAsync();
		Task<bool> ExistsAsync(int id);
		Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

		// Create
		Task AddAsync(TEntity entity);
		Task AddRangeAsync(IEnumerable<TEntity> entities);

		// Delete
		Task RemoveAsync(TEntity entity);
		Task RemoveRangeAsync(IEnumerable<TEntity> entities);
	}
}
