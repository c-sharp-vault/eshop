using Catalog.DataAccess.Managers.CatalogItems.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.API.Controllers {
	public interface ICatalogItemsController {
		Task<IActionResult> CreateSingleAsync(CreateSingleRequest request);
		Task<IActionResult> GetRangeAsync(GetRangeRequest request);
		Task<IActionResult> GetSingleAsync(GetSingleRequest request);
		Task<IActionResult> RemoveSingleAsync(RemoveSingleRequest request);
		Task<IActionResult> UpdateSingleAsync(UpdateSingleRequest request);
	}
}