using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	public interface IHaveTestData {
		void SetupTestData();
		void ClearData();
	}
}
