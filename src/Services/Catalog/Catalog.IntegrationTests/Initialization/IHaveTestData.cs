using System.Threading.Tasks;

namespace Catalog.IntegrationTests.Initialization {
	public interface IHaveTestData {
		Task SetupTestData();
	}
}
