using Catalog.DataAccess.DTOs.CatalogItem;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
	public interface ICatalogItemsController {
		Task<IActionResult> GetSingleAsync(int id);
		Task<IActionResult> GetRangeAsync(byte pageSize, byte pageIndex, bool includeNested);
		IActionResult CreateSingleAsync(CatalogItemCreateSingleDTO catalogItemDTO);
		Task<IActionResult> UpdateSingleAsync(CatalogItemUpdateDTO catalogItemUpdateDTO);
		Task<IActionResult> RemoveSingleAsync(int id);
	}
}