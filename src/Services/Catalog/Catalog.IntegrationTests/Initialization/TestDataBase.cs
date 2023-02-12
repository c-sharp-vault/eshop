
using Catalog.Core.Models;
using Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	internal abstract class TestDataBase : IHaveTestData {
		private CatalogDbContext _dbContext;

		CatalogDbContext GetDbContext() {
			if (_dbContext == null) {
				var configuration = TestHelpers.GetConfiguration();
				_dbContext = TestHelpers.GetContext(configuration);
			}

			return _dbContext;
		}

		static async Task ClearData(CatalogDbContext context, string[] tables) {
			foreach (var table in tables) {
				await context.Database.ExecuteSqlRawAsync($"DELETE FROM {table}");
				await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT (\"{table}\", RESEED, 1)");
			}
		}

		protected async Task ProcessInsert<TEntity>(List<TEntity> entities) {
			var dbContext = GetDbContext();

			if (!entities.Any()) return;

			foreach (IEntity entity in entities) {
				entity.CreatedBy = TestData.CurrentUser;
				entity.UpdatedBy = TestData.CurrentUser;
				entity.CreatedOn = TestData.CurrentDateTime;
				entity.UpdatedOn = TestData.CurrentDateTime;
			}

			IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () => {
				using (var transaction = dbContext.Database.BeginTransaction()) {
					var entityType = dbContext.Model.FindEntityType(typeof(TEntity).FullName);
					await dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");

					entities.ForEach(record => dbContext.Add(record));
					await dbContext.SaveChangesAsync();

					await dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");
					transaction.Commit();
				}
			});
		}

		protected abstract string[] Tables { get; }
		protected abstract Task SeedData();

		protected async Task ClearAndReseedDatabase() {
			if (Tables.Any()) await ClearData(GetDbContext(), Tables);

			await SeedData();
		}

		public async Task SetupTestData() {
			await ClearAndReseedDatabase();
		}
	}
}
