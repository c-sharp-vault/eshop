using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Catalog.Core.Models;

namespace Catalog.DataAccess.Repositories {
	public interface IRepository<TEntity> where TEntity : class, IEntity {

		// Read
		Task<TEntity> Get(int id);
		Task<IEnumerable<TEntity>> GetAll();
		Task<bool> Exists(int id);
		Task<IEnumerable<TEntity>> FindAll(Expression<Func<TEntity, bool>> predicate);

		// Create
		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		// Delete
		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}
}
