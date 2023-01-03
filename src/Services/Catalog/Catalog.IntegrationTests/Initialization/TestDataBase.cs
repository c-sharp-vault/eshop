
using Catalog.DataAccess;

namespace Catalog.IntegrationTests.Initialization {
	internal class TestDataBase : IHaveTestData {
		private CatalogDbContext _dbContext;

		CatalogDbContext GetDbContext() {
			if (_dbContext == null) {
				var config = TestHelpers.GetConfiguration();
			}

			return _dbContext;
		}

		public void SetupTestData() {
			throw new System.NotImplementedException();
		}
	}
}
