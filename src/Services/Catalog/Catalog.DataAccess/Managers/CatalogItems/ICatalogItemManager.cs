using Catalog.DataAccess.Managers.CatalogItems.Messages;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Managers.CatalogItems {
	public interface ICatalogItemManager {
		Task<CreateSingleResponse> CreateSingleAsync(CreateSingleRequest request);
		Task<GetRangeResponse> GetRangeAsync(GetRangeRequest request);
		Task<GetSingleResponse> GetSingleAsync(GetSingleRequest request);
		Task<RemoveSingleResponse> RemoveSingleAsync(RemoveSingleRequest request);
		Task<UpdateSingleResponse> UpdateSingleAsync(UpdateSingleRequest request);
	}
}