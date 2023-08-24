using Catalog.API.Controllers;
using Catalog.DataAccess.DTOs.CatalogItem;
using Catalog.DataAccess.Managers.CatalogItems.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccess.Managers.CatalogItems {
	public interface ICatalogItemManager {
		Task<GetSingleResponse> GetSingleAsync(int catalogItemID);
		Task<GetRangeResponse> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested);
		Task<CreateSingleResponse> CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO);
		Task<CreateRangeResponse> CreateRangeAsync(CatalogItemCreateRangeDTO catalogItemDTO);
		Task<UpdateSingleResponse> UpdateSingleAsync(CatalogItemUpdateSingleDTO catalogItemUpdateSingleDTO);
		Task<UpdateRangeResponse> UpdateRangeAsync(CatalogItemUpdateRangeDTO catalogItemUpdateRangeDTO);
		Task<RemoveSingleResponse> RemoveSingleAsync(int catalogItemID);
		Task<RemoveRangeResponse> RemoveRangeAsync(int[] ids);
	}
}