
using Catalog.Core.Models;
using Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	public abstract class TestDataBase : IHaveTestData {
		private CatalogDbContext _dbContext;

		CatalogDbContext GetDbContext() {
			if (_dbContext == null) {
				var configuration = TestHelpers.GetConfiguration();
				_dbContext = TestHelpers.GetContext(configuration);
			}

			return _dbContext;
		}

		static void ClearData(CatalogDbContext context, string[] tables) {
			foreach (var table in tables) {
				context.Database.ExecuteSqlRaw($"DELETE FROM {table}");
				context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{table}\", RESEED, 1)");
			}
		}

		protected void ProcessInsert<TEntity>(List<TEntity> entities) {
			var dbContext = GetDbContext();

			if (!entities.Any()) return;

			foreach (IEntity entity in entities) {
				entity.CreatedBy = TestData.CurrentUser;
				entity.UpdatedBy = TestData.CurrentUser;
				entity.CreatedOn = TestData.CurrentDateTime;
				entity.UpdatedOn = TestData.CurrentDateTime;
			}

			IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
			strategy.Execute(() => {
				using (var transaction = dbContext.Database.BeginTransaction()) {
					var entityType = dbContext.Model.FindEntityType(typeof(TEntity).FullName);
					dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");

					entities.ForEach(record => dbContext.Add(record));
					dbContext.SaveChanges();

					dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");
					transaction.Commit();
				}
			});
		}

		protected abstract string[] Tables { get; }
		protected abstract void SeedData();

		protected void ClearAndReseedDatabase() {
			if (Tables.Any()) ClearData(GetDbContext(), Tables);

			SeedData();
		}

		public void ClearData() {
			ClearData(GetDbContext(), Tables);
		}

		public void SetupTestData() {
			ClearAndReseedDatabase();
		}
	}
}
