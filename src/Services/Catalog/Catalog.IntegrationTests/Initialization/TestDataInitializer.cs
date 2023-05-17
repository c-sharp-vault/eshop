using Catalog.Core.Models;
using Catalog.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	public static class TestDataInitializer {
		public static async Task ClearData(CatalogDbContext dbContext) {
			string[] entities = new string[] {
				$"Catalog.{nameof(CatalogBrand)}",
				$"Catalog.{nameof(CatalogItem)}",
				$"Catalog.{nameof(CatalogType)}"
			};

			foreach (string entity in entities) {
				await dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {entity};");
				await dbContext.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT (\"{entity}\", RESEED, 1);");
			}
		}
		private static async Task SeedData(CatalogDbContext dbContext) {
			await ProcessInsert(dbContext, dbContext.CatalogBrands, TestData.CatalogBrands);
			await ProcessInsert(dbContext, dbContext.CatalogItems, TestData.CatalogItems);
			await ProcessInsert(dbContext, dbContext.CatalogTypes, TestData.CatalogTypes);
		}

		private static async Task ProcessInsert<TEntity>(CatalogDbContext dbContext, DbSet<TEntity> dbSet, List<TEntity> entities) where TEntity : Entity {
			if (dbSet.Any()) return;

			foreach (TEntity entity in entities) {
				entity.CreatedBy = TestData.CurrentUser;
				entity.UpdatedBy = TestData.CurrentUser;
				entity.CreatedOn = TestData.CurrentDateTime;
				entity.UpdatedOn = TestData.CurrentDateTime;
			}

			IExecutionStrategy executionStrategy = dbContext.Database.CreateExecutionStrategy();
			await executionStrategy.ExecuteAsync(async () => {
				using (var transaction = await dbContext.Database.BeginTransactionAsync()) {
					IEntityType entityType = dbContext.Model.FindEntityType(typeof(TEntity).FullName);
					await dbContext.Database.ExecuteSqlAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} ON");

					await dbSet.AddRangeAsync(entities);
					await dbContext.SaveChangesAsync();

					await dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} OFF");
					transaction.Commit();
				}
			});
		}

		internal static async Task ClearAndReseedDatabase(CatalogDbContext dbContext) {
			await ClearData(dbContext);
			await SeedData(dbContext);
		}
	}
}
