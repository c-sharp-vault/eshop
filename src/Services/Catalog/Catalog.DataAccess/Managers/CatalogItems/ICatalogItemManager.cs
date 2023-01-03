using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Managers.CatalogItems {
	public interface ICatalogItemManager {
		Task<CreateSingleResponse> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO);
		Task<GetRangeResponse> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested);
		Task<GetSingleResponse> GetSingleAsync(int catalogItemID);
		Task<RemoveSingleResponse> RemoveSingleAsync(int catalogItemID);
		Task<UpdateSingleResponse> UpdateSingleAsync(CatalogItemUpdateDTO catalogItemUpdateDTO);
	}
}